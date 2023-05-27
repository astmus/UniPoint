using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities.Query;
using MissBot.Handlers;
using MissCore;
using MissCore.Bot;
using MissCore.Collections;
using MissCore.Data;
using MissDataMaiden.Entities;

namespace MissDataMaiden.Commands
{

    public class SearchDataBaseHandler : InlineQueryHandler
    {
        private readonly IBotContext botContext;
        IJsonRepository repository;

        public SearchDataBaseHandler(IBotContext bot, IJsonRepository jsonRepository)
        {
            botContext = bot;
            repository = jsonRepository;
        }
         
        public async override Task LoadAsync(Paging pager, InlineResponse<InlineQuery> response, InlineQuery query, CancellationToken cancel = default)
        {
            var search = botContext.Get<Search<DataBase>>();
            
            var botUnit = await botContext.GetBotUnitAsync<DataBase>();
            var items = await repository.FindAsync<DataBase>(search with { Query = query.Query, Pager = pager, }, cancel);

            var units = items.SupplyTo<InlineQueryResult<DataBase>>();
            foreach (var u in units)
            {
                u.Title = u.Meta.GetItem(2).Format(IMetaItem.Formats.Section);
                u.Actions = botUnit.GetUnitActions(u);
            }

            response.Write(units);
            await response.Commit(default);            
        }
    }
}
