using MissBot.Abstractions;
using MissCore.Abstractions;
using MissCore.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MissBot.Handlers
{
    public class MessageHandler : BaseHandler<Message>
    {
        protected override IHandleContext Context { get; set; }

        public  Message GetDataForHandle()
            => Context.ContextData.Get<Message>();


        public bool ItCanBeHandled(IHandleContext context)
            => Context.ContextData.Get<UpdateType>() is UpdateType.Message;

        public override Task StartHandleAsync(Message data, IHandleContext context)
        {
            Console.WriteLine(data.Text);
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
