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

    public record ListDBs :  BotEntity<InlineQuery>
    {
        public string Query { get; set; }
        public record SqlQuery(string sql, int skip, int take) : SqlQuery<InlineQueryHandler.DataUnit>(sql, skip,take);
        public class QueryHandler : SqlRaw<InlineQueryHandler.DataUnit>.Handler<SqlQuery<InlineQueryHandler.DataUnit>>
        {
            public QueryHandler(IConfiguration config) : base(config)
            {
            }
        }
    }

    public class ListDiskCommandHandler : InlineQueryHandler
    {
        private readonly IConfiguration config;
        private readonly IMediator mm;
        SqlQuery<DataUnit> CurrentRequest;
        ListDBs dbs;

        public ListDiskCommandHandler(IConfiguration config, IMediator mm)
        {
            //var disk = config.GetSection(nameof(IBotCommandInfo)).GetChildren().ToList()[1].Get<Disk>();

            this.config = config;
            this.mm = mm;
            dbs = config.GetSection(nameof(IBotCommandInfo)).GetChildren().ToList()[2].Get<ListDBs>();
            CurrentRequest = new SqlQuery<DataUnit>(dbs.Query, 0, BatchSize ?? 15);
        }


        public async override Task<IEnumerable<ResultUnit>> LoadAsync(int skip, string filter)
        {
            var objs = await mm.Send(CurrentRequest with { skip = skip });
            return objs./*Where(w => w.Title.Contains(filter)).Skip(skip).Take(BatchSize ?? 15)*/
                Select(s => ResultUnit.Create(s, s.Content, Create(s.Id))) ?? new []{ ResultUnit.Empty};
        }

        InlineKeyboardMarkup Create(string id)
            => InlineKeyBoard.Create($"info;{id}", $"restore;{id}", $"delete;{id}");

    }
}
