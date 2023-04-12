using MissBot.Attributes;
using MissBot.Abstractions;
using MissDataMaiden.Queries;
using Duende.IdentityServer.Services;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.EntityFrameworkCore.Diagnostics;
using MissCore.Entities;
using Newtonsoft.Json.Serialization;
using Newtonsoft.Json;
using MediatR;
using MissBot.Common;
using MissBot.Extensions.Response;
using Telegram.Bot.Types;
using MissBot.Handlers;
using Telegram.Bot.Types.InlineQueryResults;

namespace MissDataMaiden.Commands
{

    public record ListDBs : BotEntity<InlineQuery>.Unit
    {
        public string Query { get; set; }
        public record SqlQuery(string sql, int skip, int take, string filter) : SqlQuery<ListDiskInlineUnit>.Query(sql, skip, take, filter);
        public class QueryHandler : SqlQuery.Handler<SqlQuery>
        {
            public QueryHandler(IConfiguration config) : base(config)
            {
            }
        }
    }
    public record ListDiskInlineUnit : InlineQueryHandler.InlineUnit
    {
        static ListDiskInlineUnit()
        {
            Unit<DBInfo>.Sample = new DBInfo();
            Unit<DBDelete>.Sample = new DBDelete();
            Unit<DBRestore>.Sample = new DBRestore();            
        }
        public ListDiskInlineUnit()
        {
         
        }
        protected override void Refresh()
        {
            var v = Info;
            var d = Delete;
            var r = Restore;
        }
        public DBInfo Info
            => Set(Unit<DBInfo>.Sample with { Id = this.Id });
        public DBDelete Delete
            => Set(Unit<DBDelete>.Sample with { Id = this.Id });
        public DBRestore Restore
            => Set(Unit<DBRestore>.Sample with { Id = this.Id });
    }

    public class ListDiskInlineHandler : InlineQueryHandler
    {
        private readonly IConfiguration config;
        private readonly IMediator mm;
        ListDBs.SqlQuery CurrentRequest;
        ListDBs dbs;

        public ListDiskInlineHandler(IConfiguration config, IMediator mm)
        {
            //var disk = config.GetSection(nameof(IBotCommandInfo)).GetChildren().ToList()[1].Get<Disk>();

            this.config = config;
            this.mm = mm;
            dbs = new ListDBs() { Query = config.GetSection(nameof(IBotCommandInfo)).GetChildren().ToList()[2].GetValue<string>("Query") };
            CurrentRequest = new ListDBs.SqlQuery(dbs.Query, 0, BatchSize ?? 15, "");
        }


        public async override Task<BotUnion> LoadAsync(int skip, string filter)
        {
            var objs = await mm.Send(CurrentRequest with { skip = skip, filter = filter });
            return objs;
            //ResultUnit.Create(s,filter, s.Content, Create(s.Id))) ?? new []{ ResultUnit.Empty};
        }

        //InlineKeyboardMarkup Create(string id)
        //    =>new InlineKeyBoard(new InlineAction[]{ new DBInfo(id), new DBDelete(id), new DBRestore(id) });

    }
}
