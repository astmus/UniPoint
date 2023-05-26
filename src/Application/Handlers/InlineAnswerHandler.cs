using MissBot.Abstractions;
using MissBot.Entities;
using MissBot.Entities.Results;

namespace MissBot.Handlers
{

    public abstract class InlineAnswerHandler : BaseHandler<ChosenInlineResult>
    {
        public async override Task HandleAsync(ChosenInlineResult data, CancellationToken cancel = default)
        {            
            await HandleResultAsync(Context.BotServices.Response<ChosenInlineResult>(), Context.Get<Message>(), data, cancel).ConfigureAwait(false);            
        }
        public abstract Task HandleResultAsync(IResponse<ChosenInlineResult> response, Message message, ChosenInlineResult result, CancellationToken cancel = default);
    }
}
