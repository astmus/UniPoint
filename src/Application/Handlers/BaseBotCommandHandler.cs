using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataContext;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissBot.Extensions.Entities;
using MissCore.Data;

namespace MissBot.Handlers
{
    public abstract class BaseBotCommandHandler : BaseHandler<BotCommand>, IAsyncBotCommandDispatcher
    {
        (string command, string[] args) Current;
        public  async Task ExecuteAsync(IHandleContext context)
        {
            if (context.Any<Update>() is UnitUpdate update && update.IsCommand)
            {
                Context = context;
                Current = context.Take(Id<Message>.Value).GetCommandAndArgs();
                await HandleAsync(context, Current.command);
            }
            else
                await context.CurrentHandler(context);
        }
        public override Task HandleAsync(BotCommand data, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }
        public abstract Task HandleBotCommandAsync<TCommand>(IHandleContext context, CancellationToken cancel = default) where TCommand : BotCommand, IBotUnitCommand;
        protected abstract Task HandleAsync(IHandleContext context, string command);

    }
}
