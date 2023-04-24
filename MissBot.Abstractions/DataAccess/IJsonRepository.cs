using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
    public interface IJsonRepository : IRepository
    {        
        Task<TResult> HandleQueryAsync<TResult>(SQLUnit sql, CancellationToken cancel = default) where TResult: class;
        Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(SQLUnit sql, CancellationToken cancel = default) where TResult:class;
        Task<JArray> HandleQueryGenericItemsAsync(SQLUnit sql, CancellationToken cancel = default);
        Task<JObject> HandleQueryGenericObjectAsync(SQLUnit sql, CancellationToken cancel = default);
    }
}
