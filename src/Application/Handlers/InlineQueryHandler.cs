using MissBot.Abstractions;
using MissBot.Entities.Query;
using MissBot.Response;
using MissCore.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissBot.Handlers
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record InlineQueryResult<TEntity> : InlineResultUnit
    {
        private const string empty = "Empty";         
        
    }

    public abstract class InlineQueryHandler : BaseHandler<InlineQuery>
    {
        public async override Task HandleAsync(InlineQuery data, IHandleContext context, CancellationToken cancel = default)
        {
            var response = context.BotServices.GetRequiredService<IResponse<InlineQuery>>();
    
            await LoadAsync(response, data, cancel);

            //await response.Commit(default);

        }
        public virtual int? BatchSize { get; }

        public abstract Task LoadAsync(IResponse<InlineQuery> response, InlineQuery query, CancellationToken cancel = default);
    }
}
