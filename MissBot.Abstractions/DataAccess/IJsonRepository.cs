using MissBot.DataAccess.Sql;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
    public interface IJsonRepository : IRepository
    {        
        Task<TResult> HandleQueryAsync<TResult>(SQL sql, CancellationToken cancel = default) where TResult: class;
        Task<Unit<TResult>> HandleQueryItemsAsync<TResult>(SQL sql, CancellationToken cancel = default) where TResult:class;
        Task<JArray> HandleQueryGenericItemsAsync(SQL sql, CancellationToken cancel = default);
        Task<JObject> HandleQueryGenericObjectAsync(SQL sql, CancellationToken cancel = default);
    }
}
