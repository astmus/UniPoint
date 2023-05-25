using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Entities.Query;
using MissBot.Response;
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
            var response = Context.BotServices.GetRequiredService<IResponse<InlineQuery>>();
    
            await LoadAsync(response, data, cancel);            
        }

        public abstract Task LoadAsync(IResponse<InlineQuery> response, InlineQuery query, CancellationToken cancel = default);
    }
}
