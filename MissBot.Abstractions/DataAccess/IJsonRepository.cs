using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
    public interface IJsonRepository : IRepository
    {        
        //Task<TResult> HandleQueryAsync<TResult>(IQueryUnit<TResult> query, CancellationToken cancel = default) where TResult: class;
        Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(BotRequest query, CancellationToken cancel = default) where TResult:class;
        Task<JArray> HandleQueryGenericItemsAsync(BotRequest query, CancellationToken cancel = default);
        Task<JObject> HandleQueryGenericObjectAsync(BotRequest query, CancellationToken cancel = default);
    }
}
