using MissBot.Abstractions.Entities;
using MissBot.Entities;

namespace MissBot.Abstractions
{
    public interface IUnitRequest<TUnit> : IUnitRequest
    {
        
    }
    public interface ISearchUnitRequest<TUnit> : IUnitRequest<TUnit>
    {
        
    }
    public interface IUnitRequest
    {
        IEnumerable<IUnitItem> Params { get; }
        RequestOptions Options { get; set; }
        string GetCommand();
    }
    public readonly record struct BaseParameter(string ItemName, object ItemValue) : IUnitItem
    {
        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
    public readonly record struct BaseParameter<TName, TValue>(TName ItemName, TValue ItemValue) : IUnitItem<TName, TValue>
    {
        public string Serialize()
        {
            throw new NotImplementedException();
        }
    }
    [Flags]
    public enum RequestOptions
    {
        Unknown = 0,
        JsonAuto = 1,
        JsonPath = 2,
        Scalar = 4,
        RootContent = 8
    }

    public interface IMetaCollection<TUnit> : IEnumerable<TUnit>, IMetaCollection where TUnit : class
    {
        IEnumerable<TEntity> EnumarateAs<TEntity>() where TEntity:class;
    }
    public interface IMetaValue
    {
        string Key { get; }
        object Value { get; }
    }
    public readonly record struct MetaValue(string Key, object Value) : IMetaValue
    {
    }

    public interface IMetaCollection
    {
        IEnumerable<IMetaValue> GetValues();
        IEnumerable<TUnit> BringTo<TUnit>() where TUnit : class;        
        IEnumerable<TSub> SupplyTo<TSub>() where TSub :class;
    }
}
