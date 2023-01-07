using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using MissBot.Common;
using MissBot.Extensions;
using MissCore.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MissBot.Handlers
{
    public abstract class BotCommandHandler<TCommand> : BaseHandler<TCommand> where TCommand : BotCommand
    {
        public TCommand Command { get; set; }
        public IEnumerable<TCommand> Pending { get; set; }

        public override async Task StartHandleAsync(TCommand data, IHandleContext context)
        {
            var typle = context.Data.Get<Update>().Message.GetCommandAndArgs();

            var cmdInfo = ActivatorUtilities.GetServiceOrCreateInstance<TCommand>(Context.BotServices);
            cmdInfo.Command = typle.command;
            await HandleCommandAsync(cmdInfo, typle.args);
        }

        public abstract Task HandleCommandAsync(TCommand command, string[] args);
        public override bool ItCanBeHandled(IHandleContext context)
            => context.Data.Get<Message>()?.Entities?.FirstOrDefault()?.Type is MessageEntityType.BotCommand;

    }
}
