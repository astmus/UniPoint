using MissBot.Abstractions.Entities;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
    public interface IJsonRepository : IRepository
    {
        Task<JArray> ReadUnitDataAsync<TUnit>(TUnit unit, CancellationToken cancel = default) where TUnit : IBotUnit;
        Task<ICollection<TResult>> HandleQueryResultAsync<TResult>(IRepositoryCommand cmd, CancellationToken cancel = default) where TResult : class;
        Task<JArray> HandleReadAsync(IRepositoryCommand cmd, CancellationToken cancel = default);
        Task<JObject> HandleScalarAsync(IRepositoryCommand cmd, CancellationToken cancel = default);
    }
}
