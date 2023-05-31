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
        IEnumerable<IMetaItem> Params { get; }
        RequestOptions Options { get; set; }
        string GetCommand();
    }
    public readonly record struct BaseParameter(string UnitName, object UnitValue) : IMetaItem
    {
    }
    public readonly record struct BaseParameter<TName, TValue>(TName UnitName, TValue UnitValue) : IMetaItem<TName, TValue>
    {
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
        IEnumerable<TEntity> EnumarateAs<TEntity>() where TEntity:class, IUnit<TUnit>;
    }

    public interface IMetaCollection
    {
        IEnumerable<KeyValuePair<string, object>> GetValues();
        IEnumerable<TUnit> BringTo<TUnit>() where TUnit : class;        
        IEnumerable<TSub> SupplyTo<TSub>() where TSub :class;
    }
}
