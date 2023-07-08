using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Handlers;
using MissBot.Entities;
using MissBot.Entities.Abstractions;
using MissBot.Extensions;

using MissCore.Bot;
using MissCore.Data;
using MissCore.Extensions;
using MissBot.Identity;

namespace MissCore.Handlers
{
	public abstract class BaseBotCommandDispatcher : BaseHandler<BotCommand>, IAsyncBotCommandDispatcher
	{
		protected (string command, ICollection<string> parameters) Current;
		public override sealed AsyncHandler AsyncDelegate
			=> ExecuteAsync;

		public async Task ExecuteAsync(IHandleContext context)
		{
			if (context.Any<Update>() is UnitUpdate update && update.IsCommand)
			{
				try
				{
					Current = context.Take(Id<Message>.Instance).GetCommandAndArgs();
					SetContext(context);
					await HandleAsync(context, Current.command);
				}
				catch (Exception error)
				{
					var command = context.BotServices.ResponseError();
					command.Write(error);
					await command.Commit();
				}
				context.IsHandled ??= true;
			}
			else
				await context.GetNextHandler().ConfigureAwait(false);
		}

		public abstract Task HandleBotCommandAsync<TCommand>(IHandleContext context, CancellationToken cancel = default) where TCommand : BotCommand, IBotAction;
		protected virtual async Task HandleAsync(IHandleContext context, string command)
		{
			if (context.Bot.Commands.FirstOrDefault(c => c.Action == command) is BotUnitCommand cmd)
				await HandleAsync(cmd).ConfigureAwait(false);
			else
				await context.BotServices.Response<BotCommand>().Write($"There is no handler for command {command}").Commit().ConfigFalse();
		}
	}
}
