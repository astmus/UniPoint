using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities.Query;
using MissBot.Handlers;
using MissCore;
using MissCore.Bot;
using MissCore.Collections;
using MissDataMaiden.Entities;

namespace MissDataMaiden.Commands
{

    public class SearchDataBaseHandler : InlineQueryHandler
    {
        private readonly IBotContext botContext;
        IJsonRepository repository;
        static IUnitRequest<DataBase> searchRequest;
        static Search<DataBase> search;

        public SearchDataBaseHandler(IBotContext bot, IJsonRepository jsonRepository)
        {
            botContext = bot;
            repository = jsonRepository;
        }
         
        public async override Task LoadAsync(IResponse<InlineQuery> response, InlineQuery query, CancellationToken cancel = default)
        {
            var unit = botContext.Get<Paging>();            
            search ??= unit.Find<DataBase>(query);

            var botUnit = await botContext.GetUnitAsync<DataBase>();
            var items = await repository.FindAsync<DataBase>(query.Query, query.Skip, 15, cancel);
            
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
