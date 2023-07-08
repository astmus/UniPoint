using MissBot.Abstractions.Bot;
using MissBot.Entities.Abstractions;

namespace MissBot.Abstractions.Actions
{
	public interface IMetaCollection<TData> : IMetaCollection, IEnumerable<TData> where TData : class
	{
		IEnumerable<IUnit<TData>> EnumarateUnits();
		IEnumerable<TUnit> EnumarateUnits<TUnit>() where TUnit : BaseUnit, IUnit<TData>;
		IEnumerable<TEntity> EnumarateAs<TEntity>() where TEntity : class, TData;
	}

	public interface IMetaCollection : IMetaUnit, IUnitEntity
	{
		int Count { get; }
		IEnumerable<TUnit> Enumarate<TUnit>() where TUnit : class;
	}
}
