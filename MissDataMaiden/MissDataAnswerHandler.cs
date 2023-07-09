using LinqToDB;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities;
using MissBot.Entities.Results;
using MissCore.DataAccess;
using MissCore.Handlers;

using MissDataMaiden.Entities;

namespace MissDataMaiden
{
	internal class MissDataAnswerHandler : InlineAnswerHandler
	{
		IJsonRepository repository;
		public MissDataAnswerHandler(IJsonRepository jsonRepository)
		{
			repository = jsonRepository;
		}

		public override async Task HandleResultAsync(Message message, ChosenInlineResult result, CancellationToken cancel = default)
		{
			var unit = Context.Bot.GetUnit<DataBaseInfo>();
			unit.Id = long.Parse(result.Id);
			ITable<DataBase> q = Context.Bot.GetRepository<DataBase>(unit.Template, unit.Id) as ITable<DataBase>;
			q.FirstOrDefault();

			var request = BotUnitRequest.Create(unit);
			var response = Context.BotServices.Response<DataBase>();

			var dbInfo = await repository.HandleScalarAsync<Info>(request, cancel);

			Context.Bot.UnitActions(dbInfo);

			response.WriteUnit(dbInfo);

			await response.Commit(cancel);

		}
	}
}

