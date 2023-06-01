using MissBot.Abstractions;
using MissBot.Abstractions.Handlers;
using MissBot.Entities;

namespace MissCore.Handlers
{
    public class MessageHandler : BaseHandler<Message>
    {
        

        public override Task HandleAsync(Message data, CancellationToken cancel = default)
        {
            if (Context.Any<IUnitUpdate>() is IUnitUpdate update && update.IsCommand)
                return Task.CompletedTask;
            //Console.WriteLine(data.Text);
            Context.IsHandled = true;
            return Task.CompletedTask;
        }
    }
}
