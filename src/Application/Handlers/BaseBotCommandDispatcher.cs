using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissBot.Extensions.Entities;
using MissCore.Bot;
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
                    await command.Commit();                    
                }
                if (!context.IsHandled.HasValue)
                    context.IsHandled = true;
            }
            else
                await context.MoveToNextHandler();
        }
        

        public abstract Task HandleBotCommandAsync<TCommand>(IHandleContext context, CancellationToken cancel = default) where TCommand : BotCommand, IBotUnitCommand;
        protected virtual async Task HandleAsync(IHandleContext context, string command)
        {
            if (context.Bot.Commands.FirstOrDefault(c => string.Compare(c.Action, command, true) == 0) is BotUnitCommand cmd)
            {
                //cmd.Payload = Current.command;
                await HandleAsync(cmd).ConfigureAwait(false);
            }
            else
            {
                context.BotServices.Response<BotCommand>().Content = $"There is no handler for command {command}";
                await context.BotServices.Response<BotCommand>().Commit();
            }

        }
    }
}
