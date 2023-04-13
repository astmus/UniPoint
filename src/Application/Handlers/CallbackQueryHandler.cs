using MissBot.Abstractions;
using MissBot.Extensions;
using MissCore;
using MissCore.Handlers;
using Telegram.Bot.Types;

namespace MissBot.Handlers
{
    public abstract class CallbackQueryHandler : BaseHandler<CallbackQuery>
    {
        public CallbackQueryHandler(IResponseNotification notifier)
            => this.notifier = notifier;
        
        (string command, string[] args) data;
        protected readonly IResponseNotification notifier;

        public async override Task HandleAsync(IContext<CallbackQuery> context)
        {
            var query = context.Data;
            data = query.GetCommandAndArgs();           
            notifier.Init(null, context.ClientDelegate, context.Data);

            try
            {                
                await HandleAsync(context.Root, data.command, data.args);
            }
            catch (Exception ex)
            {
                await notifier.ShowPopupAsync(ex.Message);
            }
            context.CommonUpdate.IsHandled = true;
        }

        protected abstract Task HandleAsync(IHandleContext context, string query, string[] args);
    }
}
