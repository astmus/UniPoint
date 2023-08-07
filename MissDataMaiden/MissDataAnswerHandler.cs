using LinqToDB;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities;
using MissBot.Entities.Results;
using MissCore.Bot;
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
			var unit = Context.Bot.GetUnit<BotUnit<DataBase>>();
			unit.Identifier = long.Parse(result.Id);

			//unit.Id = long.Parse(result.Id);
			//var q = Context.Bot.GetRepository<DataBase>(unit.Template, unit.Id);
			//q.FirstOrDefault();

			var request = BotUnitRequest.Create(unit);
			var response = Context.Response<DataBase>();

			var dbInfo = await repository.HandleScalarAsync<DataBaseBotUnit>(request, cancel);
			dbInfo.Identifier = unit.Identifier;

			Context.Bot.UnitActions(dbInfo);

			response.WriteUnit(dbInfo);

			await response.Commit(cancel);

		}
	}
}

