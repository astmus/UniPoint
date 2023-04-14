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
using MissDataMaiden.Entities;

namespace MissDataMaiden.Commands
{

    public class DataBasesList
    {
        public string Query { get; set; }
        public record SqlQuery(string sql, int skip, int take, string filter) : SqlQuery<DataBase>.Query(sql, skip, take, filter);
        public class QueryHandler : SqlQuery.Handler<SqlQuery>
        {
            public QueryHandler(IConfiguration config) : base(config)
            {
            }
            protected override Unit<DataBase> CreateUnit(DataBase entity)
            {
                var unit = InlineUnit<DataBase>.Instance with { Value = entity }; 
                
                unit[nameof(InlineUnit.Id)] = entity.Id;
                unit[nameof(InlineUnit.Title)] = entity.Name;
                unit[nameof(InlineUnit.Description)] = $"{entity.Size / 1024.0} Mb";

                unit[nameof(DBInfo)] =
                        Unit<DBInfo>.Sample with { Id = entity.Id, Action = nameof(DBInfo) };
                unit[nameof(DBDelete)] =
                        Unit<DBDelete>.Sample with { Id = entity.Id, Action = nameof(DBDelete) };
                unit[nameof(DBRestore)] =
                        Unit<DBRestore>.Sample with { Id = entity.Id, Action = nameof(DBRestore) };
                return unit;
                //.Instance with { Value = entity };
                //Unit < DataBase>.Sample.
            }
        }
        //public record DbListInlineUnit : InlineUnit<DataBase>
        //{
        //    protected override DataBase InvalidateMetadata(InlineUnit<DataBase> unit, DataBase entity)
        //    {
        //        //InlineUnit<DataBase>.
        //        //unit.Description = nameof(InvalidateMetadata);
        //        Set(Info);
        //        Set(Delete);
        //        return entity;
        //    }
        //    public DBInfo Info
        //        => Set(Unit<DBInfo>.Sample with { Id = this.Id, Action = nameof(DBInfo) });
        //    public DBDelete Delete
        //        => Set(Unit<DBDelete>.Sample with { Id = this.Id, Action = nameof(DBDelete) });
        //    public DBRestore Restore
        //        => Set(Unit<DBRestore>.Sample with { Id = this.Id, Action = nameof(DBRestore) });
        //}
    }


    public class ListDiskInlineHandler : InlineQueryHandler
    {
        private readonly IConfiguration config;
        private readonly IMediator mm;
        DataBasesList.SqlQuery CurrentRequest;
        DataBasesList dbs;

        public ListDiskInlineHandler(IConfiguration config, IMediator mm)
        {
            //var disk = config.GetSection(nameof(IBotCommandInfo)).GetChildren().ToList()[1].Get<Disk>();

            this.config = config;
            this.mm = mm;
            dbs = new DataBasesList() { Query = config.GetSection(nameof(IBotCommandInfo)).GetChildren().ToList()[2].GetValue<string>("Query") };
            CurrentRequest = new DataBasesList.SqlQuery(dbs.Query, 0, BatchSize ?? 15, "");
        }


        public async override Task<IEnumerable<ValueUnit>> LoadAsync(int skip, string filter)
        {
            var objs = await mm.Send(CurrentRequest with { skip = skip, filter = filter });
            return objs;//.Select( d
             //  => Unit<ListDBInlineUnit>.Sample  with { Value = d }).ToList();
           
            //ResultUnit.Create(s,filter, s.Content, Create(s.Id))) ?? new []{ ResultUnit.Empty};
        }

        //InlineKeyboardMarkup Create(string id)
        //    =>new InlineKeyBoard(new InlineAction[]{ new DBInfo(id), new DBDelete(id), new DBRestore(id) });

    }
}
