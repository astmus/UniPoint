using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Requests;

namespace MissBot.Abstractions.Response
{
    public class CallBackNotification : AnswerCallbackQueryRequest, IResponseNotification
    {
        private ICommonUpdate update;
        private BotClientDelegate sender;
        private CallbackQuery query;

        public CallBackNotification() : base(null)
        {
        }
        [JsonProperty(Required = Required.Always)]
        public new string CallbackQueryId
            => query.Id;

        public void Init(ICommonUpdate update, BotClientDelegate sender, CallbackQuery query)
        {
            this.update = update;
            this.sender = sender;
            this.query = query;            
        }

        public async Task SendTextAsync(string message, CancellationToken cancel = default)
        {
            Text = message;
            ShowAlert = false;
            await sender().SendQueryRequestAsync(this, cancel);
        }

        public async Task ShowPopupAsync(string message, CancellationToken cancel = default)
        {
            Text = message;
            ShowAlert = true;
            await sender().SendQueryRequestAsync(this, cancel);
        }
    }
}
