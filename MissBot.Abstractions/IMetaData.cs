using System.Collections;
using MissBot.Entities.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
	public enum MetaType : byte
	{
		Null,
		Empty,
		Value,
		Item,
		Unit,
		Union
	}

	public interface IUnitContext : IMetaData, IUnitEntity
	{
		JToken Root { get; }
		TEntity GetUnitEntity<TEntity>() where TEntity : class;
		IEnumerator UnitEntities { get; }
		//void SetContext<TUnitData>(TUnitData data) where TUnitData : JToken;
	}

	public interface IDataUnit : IIdentibleUnit
	{
		void SetDataContext<TUnit>(TUnit unit) where TUnit : JToken;
	}

	public interface IDataUnit<out TUnit> : IDataUnit
	{
		TUnit UnitData { get; }

		[JsonIgnore]
		IUnitContext DataContext { get; set; }
	}

	public interface IMetaData
	{
		object? this[string propertyName]
		{
			get;
		}

		IEnumerable<string> Paths { get; }
		IEnumerable<string> Names { get; }
		int Count { get; }
	}

	public interface ISerializableUnit
	{
		string Serialize();
	}

	public interface IUnitItem<TName, TValue>
	{
		TName Name { get; }
		TValue Value { get; }
	}

	public interface IUnitItem : IUnitItem<string, object>
	{

	}

}
