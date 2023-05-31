using MissBot.Abstractions.DataAccess;
using MissBot.Entities.Query;
using MissCore.Bot;
using MissCore.Handlers;
using MissCore.Response;
using MissDataMaiden.Entities;

namespace MissDataMaiden.Queries
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
            var items = await repository.HandleQueryAsync(search with { Query = query.Query, Pager = pager, }, cancel);
            var ite = botContext.GetUnitActions<DataBase>();

 //               => new InlineResultUnit < DataBase >() { Id = d.Id + query.Query, Title = d.Name, Description = d.Created });
            var units = items.SupplyTo<InlineResultUnit<DataBase>>();
            foreach (var u in units)
            {                 
                var item = u.GetItem(2);
                u.Content.Value = item.UnitName + ": " + item.UnitValue;
                u.Title = item.UnitName + ": " + item.UnitValue;
                u.Actions = botUnit.GetUnitActions(u);
                response.Write(u);
            }

            await response.Commit(default);
        }
    }
}
