using System.Linq.Expressions;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataContext
{
    public interface IJsonRepository : IRepository
    {
        Task<IMetaCollection<TUnit>> FindAsync<TUnit>(string search, uint skip = default, uint take = default, CancellationToken cancel = default) where TUnit : Unit;
        Task<IMetaCollection<TUnit>> HandleQueryAsync<TUnit>(IUnitRequest<TUnit> query, CancellationToken cancel = default) where TUnit : Unit;
        Task<IMetaCollection<TUnit>> ReadAsync<TUnit>(Expression<Predicate<TUnit>> criteria = default, CancellationToken cancel = default) where TUnit : Unit;
        Task<ICollection<TResult>> HandleRequestAsync<TResult>(IUnitRequest cmd, CancellationToken cancel = default) where TResult : class;
        Task<IMetaCollection> HandleReadAsync(IUnitRequest cmd, CancellationToken cancel = default);
        Task<JObject> HandleScalarAsync(IUnitRequest cmd, CancellationToken cancel = default);
    }
}
