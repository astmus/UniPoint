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

    public class Search
    {
        public string EntityAction { get; set; }
        public string Payload { get; set; }
        public record SqlQuery(string sql, string connectionString, Filter search) : SqlBotQuery<DataBase>.Query(sql, search.skip, search.take, search.predicat);
        public class QueryHandler : SqlBotQuery<DataBase>.Handler<SqlQuery>
        {
            public QueryHandler(IConfiguration config) : base(config)
            {
            }
            protected  override Unit<DataBase> CreateUnit(DataBase entity)
            {    
                var unit = InlineUnit<DataBase>.Instance with { Value = entity }; 
                
                unit[nameof(InlineUnit.Id)] = entity.Id;
                unit[nameof(InlineUnit.Title)] = entity.Name;
                unit[nameof(InlineUnit.Description)] = $"{entity.Size / 1024.0} Mb\nCreated: {entity.Created} ";

                //unit[nameof(DBInfo)] =
                //        Unit<DBInfo>.Sample with { Id = entity.Id, Action = nameof(DBInfo) };
                //unit[nameof(DBDelete)] =
                //        Unit<DBDelete>.Sample with { Id = entity.Id, Action = nameof(DBDelete) };
                //unit[nameof(DBRestore)] =
                //        Unit<DBRestore>.Sample with { Id = entity.Id, Action = nameof(DBRestore) };
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
        Search.SqlQuery query;
        SqlQuery<Search>.Request SearchPayload
            => SqlQuery<Search>.Request.Instance with { connectionString = config.GetConnectionString("Default"), sql = "select * from ##BotActionPayloads where EntityAction = 'Search' FOR JSON PATH, WITHOUT_ARRAY_WRAPPER" };
        static Search payload;
        Filter resultFilter;
        public ListDiskInlineHandler(IConfiguration config, IMediator mm)
        {            
            this.config = config;
            this.mm = mm;
            resultFilter ??= new Filter(0, 15, "");
            SqlQuery<Search>.Sample.EntityAction ??= nameof(Search);            
        }


        public async override Task<IEnumerable<ValueUnit>> LoadAsync(int skip, string search)
        {
            payload ??= await SearchPayload.LoadAsync();
            var query =  new  Search.SqlQuery(payload.Payload, SearchPayload.connectionString, resultFilter with { skip = skip+resultFilter.take, predicat = search });
       
            var objs = await mm.Send(query);
            return objs;
             
        }
        protected Unit<DataBase> CreateUnit(DataBase entity)
        {

            var unit = InlineUnit<DataBase>.Instance with { Value = entity };

            unit[nameof(InlineUnit.Id)] = entity.Id;
            unit[nameof(InlineUnit.Title)] = entity.Name;
            unit[nameof(InlineUnit.Description)] = $"{entity.Size / 1024.0} Mb";

            //unit[nameof(DBInfo)] =
            //        Unit<DBInfo>.Sample with { Id = entity.Id, Action = nameof(DBInfo) };
            //unit[nameof(DBDelete)] =
            //        Unit<DBDelete>.Sample with { Id = entity.Id, Action = nameof(DBDelete) };
            //unit[nameof(DBRestore)] =
            //        Unit<DBRestore>.Sample with { Id = entity.Id, Action = nameof(DBRestore) };
            return unit;
            //.Instance with { Value = entity };
            //Unit < DataBase>.Sample.
        }
       

    }
}
