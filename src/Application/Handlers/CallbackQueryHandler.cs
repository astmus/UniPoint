using MissBot.Abstractions;
using MissCore;
using MissCore.Handlers;
using Telegram.Bot.Types;

namespace MissBot.Handlers
{
    public class CallbackQueryHandler : BaseHandler<CallbackQuery>
    {
       

        public override Task ExecuteAsync(CancellationToken cancel = default)
            => Task.CompletedTask;

        public override Task HandleAsync(IContext<CallbackQuery> context)
        {
            throw new NotImplementedException();
        }

        
    }
}
