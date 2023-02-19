using MissBot.Abstractions;
using MissCore;
using MissCore.Abstractions;
using MissCore.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MissBot.Handlers
{
    public class CallbackQueryHandler : BaseHandler<IUpdateCallbackQuery>
    {
        protected override IHandleContext Context { get; set; }

        public virtual Task StartHandleAsync(CallbackQuery data, IHandleContext context)
        {
            Console.WriteLine($"Unhandled callbackquery {data}");
            return Task.CompletedTask;
        }

        public override async Task StartHandleAsync(IUpdateCallbackQuery data, IHandleContext context)
        {
            if (data.CallbackQuery is CallbackQuery query)
                await StartHandleAsync(query, context);        
        }

        //data.ChatId,
        //"*PONG*"//,
        //ParseMode.Markdown,
        //replyToMessageId: msg.MessageId,
        //replyMarkup: new InlineKeyboardMarkup(
        //    InlineKeyboardButton.WithCallbackData("Ping", "PONG")
        //)

    }
}
