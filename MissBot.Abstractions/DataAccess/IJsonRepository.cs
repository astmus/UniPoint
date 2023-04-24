using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
    public interface IJsonRepository : IRepository
    {        
        Task<TResult> HandleQueryAsync<TResult>(ISQLUnit sql, CancellationToken cancel = default) where TResult: class;
        Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(ISQLUnit sql, CancellationToken cancel = default) where TResult:class;
        Task<JArray> HandleQueryGenericItemsAsync(ISQLUnit sql, CancellationToken cancel = default);
        Task<JObject> HandleQueryGenericObjectAsync(ISQLUnit sql, CancellationToken cancel = default);
    }
}
