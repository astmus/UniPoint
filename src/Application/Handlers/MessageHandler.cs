using MissBot.Abstractions;
using MissCore.Abstractions;
using MissCore.Handlers;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MissBot.Handlers
{
    public class MessageHandler : BaseHandler<Message>
    {
        
        public  Message GetDataForHandle()
            => Context.Get<Message>();


        public bool ItCanBeHandled(IHandleContext context)
            => Context.Get<UpdateType>() is UpdateType.Message;

        public  Task ExecuteAsync(Message data, IHandleContext context)
        {
            Console.WriteLine(data.Text);
            return Task.CompletedTask;
        }

        public override Task ExecuteAsync(CancellationToken cancel = default)
        {
            throw new NotImplementedException();
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
