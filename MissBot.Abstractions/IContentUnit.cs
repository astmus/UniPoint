using MissBot.Abstractions.Actions;
using MissBot.Entities.Abstractions;

namespace MissBot.Abstractions
{
	public interface IContentUnit
	{
		IEnumerable<TUnit> EnumerateAs<TUnit>() where TUnit : class;
	}
	public interface IUnitCollection<TEntity> : IMetaCollection, IEnumerable<TEntity>, IUnitContainable<TEntity>, IUnitEntity where TEntity : class
	{
		IUnitActions<TEntity> Actions { get; set; }
		//IEnumerable<TAction> EnumarateActions<TAction>(IIdentible context) where TAction : IUnitAction<TEntity>;
	}

	public interface IContentUnit<TEntity> : IContentUnit where TEntity : class
	{
		bool IsEmpty
			=> Content is null || Content.Count == 0;

		IUnitCollection<TEntity> Content { get; }
	}
	public interface IUnitContainable<in TEntity> : IUnitEntity where TEntity : class
	{
		void Add(IUnit<TEntity> unit);
		void Add<TUnit>(TUnit unit) where TUnit : IUnit<TEntity>;
	}
}
