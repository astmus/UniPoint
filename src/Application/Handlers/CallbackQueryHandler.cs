using MissBot.Abstractions;
using MissCore;
using MissCore.Handlers;
using Telegram.Bot.Types;

namespace MissBot.Handlers
{
    public class CallbackQueryHandler : BaseHandler<IUpdateCallbackQuery>
    {
       

        public override Task ExecuteAsync(CancellationToken cancel = default)
            => Task.CompletedTask;

        public virtual Task StartHandleAsync(CallbackQuery data, IHandleContext context)
        {
            Console.WriteLine($"Unhandled callbackquery {data}");
            return Task.CompletedTask;
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
