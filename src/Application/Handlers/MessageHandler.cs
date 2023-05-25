using MissBot.Abstractions;
using MissBot.Entities;

namespace MissBot.Handlers
{
    public class MessageHandler : BaseHandler<Message>
    {
        public Task ExecuteAsync(Message data, IHandleContext context)
        {
            Console.WriteLine(data.Text);
            return Task.CompletedTask;
        }

        public override Task HandleAsync(Message data,  CancellationToken cancel = default)
        {
            if (Context.Any<IUnitUpdate>() is IUnitUpdate update && update.IsCommand)
                return Task.CompletedTask;
            Console.WriteLine(data.Text);
            Context.IsHandled = false;
            return Task.CompletedTask;
        }
    }
}
