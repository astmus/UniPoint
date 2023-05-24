using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataContext;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissBot.Extensions.Entities;
using MissCore.Data;

namespace MissBot.Handlers
{
    public abstract class BaseBotCommandDispatcher : BaseHandler<BotCommand>, IAsyncBotCommandDispatcher
    {
        protected (string command, string[] args) Current;
        public override sealed AsyncHandler AsyncDelegate
            =>ExecuteAsync;

        public  async Task ExecuteAsync(IHandleContext context)
        {
            if (context.Any<Update>() is UnitUpdate update && update.IsCommand)
            {
                try
                {
                    Current = context.Take(Id<Message>.Value).GetCommandAndArgs();
                    SetContext(context);
                    await HandleAsync(context, Current.command);
                }
                catch (Exception error)
                {
                    var command = context.BotServices.ErrorResponse();
                    command.Write(error);
                    await context.BotServices.Client.MakeRequestAsync(command);
                }
                if (!context.IsHandled.HasValue)
                    context.IsHandled = true;
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
        protected abstract Task HandleCommonAsync(IHandleContext context);

    }
}
