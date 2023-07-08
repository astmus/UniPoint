using System.Collections;
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

    public interface IUnitContext : IMetaData
    {
        JToken Root { get; }
        TEntity GetUnitEntity<TEntity>() where TEntity : class;
        IEnumerator UnitEntities { get; }
        //void SetContext<TUnitData>(TUnitData data) where TUnitData : JToken;
    }

    public interface IUnitContext<TUnit>
    {
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

    public interface ISerializableItem
    {
        string Serialize();
    }

    public interface IUnitItem<TName, TValue>
    {
        TName ItemName { get; }
        TValue ItemValue { get; }
    }
    public interface IUnitItem : IUnitItem<string, object>, ISerializableItem
    {

    }

}
