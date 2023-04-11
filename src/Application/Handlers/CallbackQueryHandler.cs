using MissBot.Abstractions;
using MissBot.Extensions;
using MissCore;
using MissCore.Handlers;
using Telegram.Bot.Types;

namespace MissBot.Handlers
{
    public abstract class CallbackQueryHandler : BaseHandler<CallbackQuery>
    {
        (string command, string[] args) data;
        public async override Task HandleAsync(IContext<CallbackQuery> context)
        {
            var query = context.Data;
            data = query.GetCommandAndArgs();
            await  HandleAsync(context.Root, data.command, data.args);
            context.CommonUpdate.IsHandled = true;
        }

        protected abstract Task HandleAsync(IHandleContext context, string query, string[] args);
    }
}
