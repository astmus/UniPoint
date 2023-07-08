using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.Query;
using MissBot.Extensions;
using MissCore.Data.Collections;
using MissCore.Handlers;

using MissDataMaiden.Entities;
using MissBot.Identity;

namespace MissDataMaiden
{
	internal class MissDataCallBackDispatcher : CallbackQueryHandler
	{
		public MissDataCallBackDispatcher(IResponseNotification notifier) : base(notifier)
		{ }

		IInteraction<DataBase> response;
		protected override async Task HandleAsync(string command, string unit, string id, CallbackQuery query, CancellationToken cancel = default)
		{
			response = Context.BotServices.InteractionOf<DataBase>();
			var unitAction = Context.Set(await Context.Bot.GetActionAsync<DataBase>(command));

			await notifier.Complete().ConfigFalse();

			unitAction.Id = Id<DataBase>.Instance.With(id);
			var handler = Context.GetBotService<InputParametersHandler>();
			await handler.HandleAsync(ParametersEntered, unitAction, Context, cancel);

			Context.SetNextHandler(handler.AsyncDelegate);
		}

		async Task ParametersEntered(FormattableUnitBase result, IHandleContext context)
		{
			//await response.CompleteInteraction(result).Commit();

			var repository = Context.GetBotService<IJsonRepository>();

			var unitRes = await repository.RawAsync<GenericUnit>(result.Format, default, result.ToArray());
			if (unitRes != null)
				await response.CompleteInteraction(unitRes.Content).Commit();
			else
				await response.CompleteInteraction("Not found").Commit();
			Context.IsHandled = true;
		}
	}
}
