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
    [JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public record InlineQueryResult<TEntity> : InlineResultUnit, IUnit<TEntity>
    {
        private const string empty = "Empty";        
    }

    public abstract class InlineQueryHandler : BaseHandler<InlineQuery>
    {
        public async override Task HandleAsync(InlineQuery data, CancellationToken cancel = default)
        {
            var paging = Context.Bot.Get<Paging>() with { Page = data.Page };            
            
            var response = Context.BotServices.GetRequiredService<InlineResponse<InlineQuery>>();            
            response.Pager = paging;
    
            await LoadAsync(paging, response, data, cancel);
            Context.IsHandled = true;
        }

        public abstract Task LoadAsync(Paging pager, InlineResponse<InlineQuery> response, InlineQuery query, CancellationToken cancel = default);
    }
}
