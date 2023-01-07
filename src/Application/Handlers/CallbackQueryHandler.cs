using MissCore.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MissBot.Handlers
{
    public class CallbackQueryHandler : BaseHandler<CallbackQuery>
    {
        public override CallbackQuery GetDataForHandle()
            => Context.Data.Get<Update>().CallbackQuery;

        public override bool ItCanBeHandled(IHandleContext context)
            => context.Data.Get<UpdateType>() is  UpdateType.CallbackQuery;


        public override Task StartHandleAsync(CallbackQuery data, IHandleContext context)
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
