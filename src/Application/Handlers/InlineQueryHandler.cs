using MissBot.Abstractions;
using MissBot.Entities.Query;
using MissBot.Entities.Results.Inline;
using MissCore.Handlers;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissBot.Handlers
{

    //    private const string empty = "Empty";
    //    public string Id { get ; set; }
    //    public string Title { get ; set; }
    //    //public virtual object Content
    //    //        => $"{Id}\n{Title}\n{Description}"; 
    //    public string Description { get ; set; }

    //    public static readonly InlineUnit Empty
    //        = new InlineUnit() { Id = "0", Title = "Not found" };
    //}
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record InlineQueryResult<TEntity> : InlineResultUnit
    {
        private const string empty = "Empty";         
        
    } 

    public abstract class InlineQueryHandler : BaseHandler<InlineQuery>
    {
        public async override Task HandleAsync(InlineQuery data, IHandleContext context)
        {
            var response = context.BotServices.GetRequiredService<IResponse<InlineQuery>>();
    
            await LoadAsync(response, data);

            //await response.Commit(default);

        }
        public virtual int? BatchSize { get; }

        public abstract Task LoadAsync(IResponse<InlineQuery> response, InlineQuery query);
    }
}
