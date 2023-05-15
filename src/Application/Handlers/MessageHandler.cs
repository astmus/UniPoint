using MissBot.Abstractions;
using MissBot.Entities;
using MissCore.Handlers;



namespace MissBot.Handlers
{
    public class MessageHandler : BaseHandler<Message>
    {


        public Task ExecuteAsync(Message data, IHandleContext context)
        {
            Console.WriteLine(data.Text);
            return Task.CompletedTask;
        }

        public override Task HandleAsync(Message data, IHandleContext context, CancellationToken cancel = default)
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
