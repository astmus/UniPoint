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
using MissBot.Abstractions.DataAccess;
using MissCore.Bot;
using MissBot.DataAccess.Sql;

namespace MissDataMaiden.Commands
{

    //public class Search
    //{
    //    public string EntityAction { get; set; }
    //    public string Payload { get; set; }
 
    //    //public record DbListInlineUnit : InlineUnit<DataBase>
    //    //{
    //    //    protected override DataBase InvalidateMetadata(InlineUnit<DataBase> unit, DataBase entity)
    //    //    {
    //    //        //InlineUnit<DataBase>.
    //    //        //unit.Description = nameof(InvalidateMetadata);
    //    //        Set(Info);
    //    //        Set(Delete);
    //    //        return entity;
    //    //    }
    //    //    public DBInfo Info
    //    //        => Set(Unit<DBInfo>.Sample with { Id = this.Id, Action = nameof(DBInfo) });
    //    //    public DBDelete Delete
    //    //        => Set(Unit<DBDelete>.Sample with { Id = this.Id, Action = nameof(DBDelete) });
    //    //    public DBRestore Restore
    //    //        => Set(Unit<DBRestore>.Sample with { Id = this.Id, Action = nameof(DBRestore) });
    //    //}
    //}


    public class ListDiskInlineHandler : InlineQueryHandler
    {
        private readonly IConfiguration config;
        private readonly IMediator mm;

        IJsonRepository repository;
        static Search<DataBase> payload;
        Filter resultFilter;
        public ListDiskInlineHandler(IConfiguration config, IMediator mm, IJsonRepository jsonRepository)
        {            
            this.config = config;
            this.mm = mm;
            repository = jsonRepository;            
        }

        public async override Task<IEnumerable<ValueUnit>> LoadAsync(int skip, string search)
        {
          
            payload ??=  (await  repository.HandleQueryAsync<Unit<Search<DataBase>>>(BotContext.Search.Request)).Content.FirstOrDefault();

            resultFilter = payload.Filter;
            var items = await repository.HandleQueryItemsAsync<InlineDataBase>(
                payload.ToQuery(resultFilter with { skip = skip + resultFilter.take, predicat = search ?? "" }));
         
            return items?.Content;
             
        }
     
        }
       

    }
