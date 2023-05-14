using MissBot.Abstractions.Entities;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
    public interface IJsonRepository : IRepository
    {
        Task<JArray> GetUnitDataAsync<TUnit>(TUnit unit, CancellationToken cancel = default) where TUnit : IBotUnit;
        Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(IRepositoryCommand cmd, CancellationToken cancel = default) where TResult : class;
        Task<JArray> HandleQueryGenericItemsAsync(IRepositoryCommand cmd, CancellationToken cancel = default);
        Task<JObject> HandleQueryGenericObjectAsync(IRepositoryCommand cmd, CancellationToken cancel = default);
    }
}
