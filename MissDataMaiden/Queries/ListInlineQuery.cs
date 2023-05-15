using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities.Query;
using MissBot.Handlers;
using MissCore;
using MissCore.Collections;
using MissCore.Features;

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

        public async override Task LoadAsync(IResponse<InlineQuery> response, InlineQuery query, CancellationToken cancel = default)
        {
            searchResutst ??= await botRepository.HandleQueryAsync<Search<Unit>>(BotUnit<Search>.Query(s=> { }));

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
