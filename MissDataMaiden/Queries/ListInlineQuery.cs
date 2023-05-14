using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Common;
using MissBot.Entities.Query;
using MissBot.Entities.Response;
using MissBot.Handlers;
using MissCore.Features;
using MissDataMaiden.Entities;

namespace MissDataMaiden.Commands
{

    public class SearchDataBaseHandler : InlineQueryHandler
    {
        private readonly IConfiguration config;
        private readonly IBotRepository botRepository;
        IJsonRepository repository;
        static Search<Unit> searchResutst;
        public SearchDataBaseHandler(IBotRepository bot, IJsonRepository jsonRepository)
        {
            botRepository = bot;
            repository = jsonRepository;
        }

        public async override Task LoadAsync(IResponse<InlineQuery> response, InlineQuery query)
        {
            searchResutst ??= await botRepository.HandleQueryAsync<Search<Unit>>(SqlUnit.Entity<Search>());

            query.Skrip();
            searchResutst.Query = query;

            var items = await repository.HandleQueryGenericItemsAsync(searchResutst);
            var metaItems = new MetaCollection(items);
            var units = metaItems.LandOn<InlineQueryResult<Unit>>();

            
            // searchResutst.ToQuery(resultFilter with { skip = skip + resultFilter.take, predicat = search ?? "" }));
            response.Write(units);
            await response.Commit(default);
            //else
            //    response.Write(Unit<InlineQuery>.Instance);            
        }
    }
}
