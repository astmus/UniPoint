using System.Linq.Expressions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.DataAccess
{
	public interface IJsonRepository : IRepository
	{
		//  Task<IMetaCollection<TUnit>> FindAsync<TUnit>(ISearchUnitRequest<TUnit> query, CancellationToken cancel = default) where TUnit : BaseUnit;
		Task<IMetaCollection<TUnit>> HandleQueryJsonAsync<TUnit>(IUnitRequest query, CancellationToken cancel = default) where TUnit : class;
		// Task<ICollection<TResult>> ReadCollectionAsync<TResult>(IUnitRequest cmd, CancellationToken cancel = default) where TResult : class;
		//Task<IMetaCollection> ReadAsync(IUnitRequest cmd, CancellationToken cancel = default);
		Task<JObject> HandleScalarAsync(IUnitRequest cmd, CancellationToken cancel = default);
		Task<TResult> HandleScalarAsync<TResult>(IUnitRequest request, CancellationToken cancel = default) where TResult : BaseBotUnit, IDataUnit;
		Task<IContentUnit<TResult>> RawAsync<TResult>(string request, CancellationToken cancel = default, params KeyValuePair<object, object>[] parameters) where TResult : class;

	}
}
