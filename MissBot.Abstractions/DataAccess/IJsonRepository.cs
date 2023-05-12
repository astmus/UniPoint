using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
    public interface IJsonRepository : IRepository
    {        
        //Task<TResult> HandleQueryAsync<TResult>(IQueryUnit<TResult> query, CancellationToken cancel = default) where TResult: class;
        Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(IRepositoryCommand query, CancellationToken cancel = default) where TResult:class;
        Task<JArray> HandleQueryGenericItemsAsync(IRepositoryCommand query, CancellationToken cancel = default);
        Task<JObject> HandleQueryGenericObjectAsync(IRepositoryCommand query, CancellationToken cancel = default);
    }
}
