using System.Collections;
using MissBot.Entities.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
	public interface IUnit<out TData> : IUnit where TData : class
	{
		[JsonIgnore]
		TData UnitData { get; }
		void SetDataContext<TRoot>(TRoot data) where TRoot : JToken;
	}

	public interface IUnit : IIdentibleUnit, IUnitEntity, IEnumerable
	{
		[JsonIgnore]
		IEnumerator UnitEntities { get; }
	}
}
