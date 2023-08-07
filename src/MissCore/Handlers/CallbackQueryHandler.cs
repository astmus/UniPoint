using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Handlers;
using MissBot.Entities.Abstractions;
using MissBot.Entities.Query;
using MissBot.Extensions;
using MissCore.Extensions;
using MissCore.Storage;

namespace MissCore.Handlers
{
	public abstract class CallbackQueryHandler : BaseHandler<CallbackQuery>
	{
		public CallbackQueryHandler(IResponseNotification notifier)
			=> this.notifier = notifier;

		protected (string unit, string command, string[] args) data;
		protected readonly IResponseNotification notifier;
		protected BotDataContext BotStorage { get; private set; }
		public async override Task HandleAsync(CallbackQuery query, CancellationToken cancel = default)
		{

			try
			{
				data = query.GetCommandArguments();
				await notifier.Complete().ConfigFalse();

				await HandleAsync(data.command, data.unit, data.args[0], query, cancel).ConfigureAwait(false);
			}
			catch (Exception ex)
			{
				Context.IsHandled = true;
				await notifier.ShowPopupAsync(ex.Message);
			}
		}

		protected abstract Task HandleAsync(string command, string unit, string id, CallbackQuery query, CancellationToken cancel = default);

		protected virtual void HandleUnitAction<TUnit>(IUnitAction<TUnit> action) where TUnit : BaseUnit
		{
			var handler = Context.GetBotService<IAsyncUnitActionHanlder<TUnit>>();
			if (handler == null) return;
			ThreadPool.QueueUserWorkItem<IUnitAction<TUnit>>(handler.HandleUnitAction, action, false);
		}
	}
}
