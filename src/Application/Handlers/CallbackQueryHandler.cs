using MissBot.Abstractions;
using MissBot.Entities.Query;
using MissBot.Extensions;
using MissCore.Handlers;

namespace MissBot.Handlers
{
    public abstract class CallbackQueryHandler : BaseHandler<CallbackQuery>
    {
        public CallbackQueryHandler(IResponseNotification notifier)
            => this.notifier = notifier;

        (string command, string[] args) data;
        protected readonly IResponseNotification notifier;

        public async override Task HandleAsync(CallbackQuery query, IHandleContext context)
        {

            data = query.GetCommandAndArgs();
            //notifier.Init(null, context.ClientDelegate, context.Data);

            try
            {
                await HandleAsync(context, data.command, data.args);
            }
            catch (Exception ex)
            {
                await notifier.ShowPopupAsync(ex.Message);
            }
            //context.CommonUpdate.IsHandled = true;
        }

        protected abstract Task HandleAsync(IHandleContext context, string query, string[] args);
    }
}
