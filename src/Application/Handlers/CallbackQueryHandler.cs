using MissBot.Abstractions;
using MissBot.Extensions;
using MissCore;
using MissCore.Handlers;
using Telegram.Bot.Types;

namespace MissBot.Handlers
{
    public class CallbackQueryHandler : BaseHandler<CallbackQuery>
    {

        public override Task HandleAsync(IContext<CallbackQuery> context)
        {
            var query = context.Data;
            query.GetCommandAndArgs();
            throw new NotImplementedException();
        }        
    }
}
