using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissCore.Bot;
using MissCore.Response;

namespace MissCore.Handlers
{
    public abstract class InlineQueryHandler<TUnit> : BaseHandler<InlineQuery<TUnit>> where TUnit:BaseUnit,IBotEntity
    {
        public async override Task HandleAsync(InlineQuery<TUnit> query, CancellationToken cancel = default)
        {
            var q = Context.Get<InlineQuery>();
            var paging = Context.Bot.Get<Paging>() with { Page = q.Page };

            var response = Context.BotServices.Activate<InlineResponse<InlineQuery<TUnit>>>();
            response.Pager = paging;

            await LoadAsync(paging, response, q, cancel);
            Context.IsHandled = true;
        }

        public abstract Task LoadAsync(Paging pager, InlineResponse<InlineQuery<TUnit>> response, InlineQuery query, CancellationToken cancel = default);
    }
}
