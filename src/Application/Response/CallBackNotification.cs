using MissBot.Entities.Query;

namespace MissBot.Abstractions.Response
{
    public class CallBackNotification : AnswerCallbackQueryRequest, IResponseNotification
    {
        private readonly IHandleContext context;
        public CallBackNotification(IHandleContext context) : base(null)
        {
            this.context = context;
        }

        [JsonProperty(Required = Required.Always)]
        public override string CallbackQueryId
            => context.Get<CallbackQuery>().Id;

        public async Task SendTextAsync(string message, CancellationToken cancel = default)
        {
            Text = message;
            ShowAlert = false;
            bool b = await context.BotServices.Client.SendQueryRequestAsync(this, cancel);
        }

        public async Task ShowPopupAsync(string message, CancellationToken cancel = default)
        {
            Text = message;
            ShowAlert = true;
            await context.BotServices.Client.SendQueryRequestAsync(this, cancel);
        }
    }
}
