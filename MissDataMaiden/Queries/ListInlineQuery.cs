using MediatR;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Handlers;
using MissCore.Features;
using MissDataMaiden.Entities;
using Telegram.Bot.Types;

namespace MissDataMaiden.Commands
{
     
    public class SearchDataBaseHandler : InlineQueryHandler
    {
        private readonly IConfiguration config;


        IJsonRepository repository;
        static Search<DataBase> searchResutst;
        Filter resultFilter;
        public SearchDataBaseHandler(IConfiguration config,  IJsonRepository jsonRepository)
        {
            this.config = config;
            repository = jsonRepository;
        }
        
        public async override Task LoadAsync(int skip, string search, IResponse<InlineQuery> response, InlineQuery query)
        {
            searchResutst ??= (await repository.HandleCommandAsync<Search<DataBase>>(MissBot.Abstractions.DataAccess.Unit.Entity<Search>()));
            
            resultFilter = searchResutst.Filter;
           // BotContext.Actions<BotActionRequest>(
           
 


            var items = await repository.HandleQueryItemsAsync<InlineUnit<DataBase>>(new BotRequest());
               // searchResutst.ToQuery(resultFilter with { skip = skip + resultFilter.take, predicat = search ?? "" }));

            if (items?.Count() > 0)
                response.Write(items);
            //else
            //    response.Write(Unit<InlineQuery>.Instance);            
        }        
    }
}
