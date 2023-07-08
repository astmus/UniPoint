using MissBot.Abstractions.Handlers;
using MissBot.Entities;
using MissBot.Entities.Results;

namespace MissCore.Handlers
{

    public abstract class InlineAnswerHandler : BaseHandler<ChosenInlineResult>
    {
        public async override Task HandleAsync(ChosenInlineResult data, CancellationToken cancel = default)
        {
            await HandleResultAsync(Context.Any<Message>(), data, cancel).ConfigureAwait(false);
        }
        public abstract Task HandleResultAsync(Message message, ChosenInlineResult result, CancellationToken cancel = default);
    }
}
