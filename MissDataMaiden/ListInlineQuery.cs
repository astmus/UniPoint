using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities.Query;
using MissCore.Actions;
using MissCore.Bot;
using MissCore.Data;
using MissCore.Data.Collections;
using MissCore.Handlers;
using MissCore.Response;
using MissDataMaiden.Entities;

namespace MissDataMaiden
{

	public class SearchDataBaseHandler : InlineQueryHandler<DataBase>
	{
		private readonly IBotContext botContext;
		IJsonRepository repository;

		public SearchDataBaseHandler(IBotContext bot, IJsonRepository jsonRepository)
		{
			botContext = bot;
			repository = jsonRepository;
		}

		public async override Task LoadAsync(Paging pager, InlineResponse<DataBase> response, InlineQuery query, CancellationToken cancel = default)
		{
			var search = botContext.SearchItems<DataBase>();// as Search<DataBase>) with { Query = query.Query, Pager = pager, };
			search.Query = query.Query;

			var result = await repository.HandleQueryAsync<DataBase>(search, cancel);

			if (result is IContentUnit<DataBase> dbUnits)
				foreach (var u in dbUnits.EnumerateAs<InlineResultUnit<DataBase>>())
				{
					//botContext.UnitActions(u);
					response.WriteUnit(u);
				}

			await response.Commit(default);
		}
	}
}
