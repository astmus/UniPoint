using MissBot.Abstractions;
using MissBot.Abstractions.Handlers;
using MissBot.Entities.Query;
using MissCore.Extensions;

namespace MissCore.Handlers
{
    public abstract class CallbackQueryHandler : BaseHandler<CallbackQuery>
    {
        public CallbackQueryHandler(IResponseNotification notifier)
            => this.notifier = notifier;

        protected (string command, string[] args) data;
        protected readonly IResponseNotification notifier;

        public async override Task HandleAsync(CallbackQuery query, CancellationToken cancel = default)
        {
            data = query.GetCommandAndArgs();
            //var response = Context.BotServices.Response<CallbackQuery>();
            try
            {
                await HandleAsync(data.command, data.args[0], data.args[1], query, cancel).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Context.IsHandled = true;
                await notifier.ShowPopupAsync(ex.Message);
            }
        }

        protected abstract Task HandleAsync(string command, string unit, string id, CallbackQuery query, CancellationToken cancel = default);
    }
}
