using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
    public interface IJsonRepository : IRepository
    {        
        Task<TResult> HandleQueryAsync<TResult>(string sql, CancellationToken cancel = default) where TResult: class;
        Task<IList<TResult>> HandleQueryItemsAsync<TResult>(string sql, CancellationToken cancel = default) where TResult:class;
        Task<JArray> HandleQueryGenericItemsAsync(string sql, CancellationToken cancel = default);
        Task<JObject> HandleQueryGenericObjectAsync(string sql, CancellationToken cancel = default);
    }
}
