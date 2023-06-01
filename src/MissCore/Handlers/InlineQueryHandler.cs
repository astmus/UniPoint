using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Handlers;
using MissBot.Entities.Query;
using MissCore.Bot;
using MissCore.Response;

namespace MissCore.Handlers
{
    public abstract class InlineQueryHandler<TUnit> : BaseHandler<InlineQuery<TUnit>> where TUnit:BaseUnit,IBotEntity
    {
        public async override Task HandleAsync(InlineQuery<TUnit> query, CancellationToken cancel = default)
        {

            var paging = Context.Bot.Get<Paging>() with { Page = query.Page };

            var response = Context.BotServices.Activate<InlineResponse<TUnit>>();
            response.Pager = paging;

            await LoadAsync(paging, response, query, cancel);
            Context.IsHandled = true;
        }

        public abstract Task LoadAsync(Paging pager, InlineResponse<TUnit> response, InlineQuery query, CancellationToken cancel = default);
    }
}
