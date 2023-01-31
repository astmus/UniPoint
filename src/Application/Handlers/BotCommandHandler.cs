using Microsoft.Extensions.DependencyInjection;
using MissBot.Extensions;
using MissCore.Abstractions;
using MissCore.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MissBot.Handlers
{
    public abstract class BotCommandHandler<TCommand> : BaseHandler<TCommand>, IAsyncHandler<TCommand> where TCommand : BotCommand
    {
        public TCommand Command { get; set; }
        public IEnumerable<TCommand> Pending { get; set; }

        public override async Task StartHandleAsync(TCommand data, IHandleContext context)
        {
            var typle = context.ContextData.Get<Update>().Message.GetCommandAndArgs();

            var cmdInfo = ActivatorUtilities.GetServiceOrCreateInstance<TCommand>(Context.BotServices);
            cmdInfo.Command = typle.command;
            await HandleCommandAsync(cmdInfo, typle.args);
        }

        public abstract Task HandleCommandAsync(TCommand command, string[] args);
        public override bool ItCanBeHandled(IHandleContext context)
            => context.ContextData.Get<Message>()?.Entities?.FirstOrDefault()?.Type is MessageEntityType.BotCommand;

        public Task HandleAsync(IContext<TCommand> context, TCommand data)
        {
            throw new NotImplementedException();
        }
    }
}
