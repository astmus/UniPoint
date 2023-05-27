using System.Linq.Expressions;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
    public interface IJsonRepository : IRepository
    {
        Task<IMetaCollection<TUnit>> FindAsync<TUnit>(ISearchUnitRequest<TUnit> query, CancellationToken cancel = default) where TUnit : UnitBase;
        Task<IMetaCollection<TUnit>> HandleQueryAsync<TUnit>(IUnitRequest<TUnit> query, CancellationToken cancel = default) where TUnit : UnitBase;
        Task<ICollection<TResult>> ReadCollectionAsync<TResult>(IUnitRequest cmd, CancellationToken cancel = default) where TResult : class;
        Task<IMetaCollection> ReadAsync(IUnitRequest cmd, CancellationToken cancel = default);
        Task<JObject> HandleScalarAsync(IUnitRequest cmd, CancellationToken cancel = default);
        Task<IContentUnit<TResult>> RawAsync<TResult>(string request, CancellationToken cancel = default, params KeyValuePair<object, object>[] parameters) where TResult : class;
    }
}
