using MissBot.Abstractions;
using MissBot.Entities;
using MissBot.Entities.Query;
using MissBot.Extensions;


namespace MissBot.Handlers
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
            var response = Context.BotServices.Response<CallbackQuery>();
            try
            {
                await HandleAsync(data.command, data.args[0], data.args[1], response, query, cancel).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                Context.IsHandled = true;
                await notifier.ShowPopupAsync(ex.Message);
            }            
        }

        protected abstract Task HandleAsync(string command, string unit, string id, IResponse<CallbackQuery> response,  CallbackQuery query, CancellationToken cancel = default);
    }
}
