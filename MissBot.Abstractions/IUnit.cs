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
        void SetContextRoot<TRoot>(TRoot data) where TRoot : JToken;
    }

    public interface IUnit : IIdentibleUnit, IUnitEntity
    {
        [JsonIgnore]
        IEnumerator UnitEntities { get; }
        void SetContext(object data);
    }
}
