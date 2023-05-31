using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Entities.Query;
using MissBot.Response;
using MissCore.Bot;
using MissCore.Data;
using MissCore.Data.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissBot.Handlers
{
   

    public abstract class InlineQueryHandler : BaseHandler<InlineQuery>
    {
        public async override Task HandleAsync(InlineQuery query, CancellationToken cancel = default)
        {
            var paging = Context.Bot.Get<Paging>() with { Page = query.Page };            
            
            var response = Context.BotServices.GetRequiredService<InlineResponse<InlineQuery>>();            
            response.Pager = paging;
    
            await LoadAsync(paging, response, query, cancel);
            Context.IsHandled = true;
        }

        public abstract Task LoadAsync(Paging pager, InlineResponse<InlineQuery> response, InlineQuery query, CancellationToken cancel = default);
    }
}
