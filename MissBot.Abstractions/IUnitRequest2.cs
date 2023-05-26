using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions
{
    public interface IUnitRequest<TUnit> : IUnitRequest
    {
    }
    public interface ISearchUnitRequest<TUnit> : IUnitRequest<TUnit>
    {
        
    }

    public interface IUnitRequest2 : IUnitRequest
    {        
        string Template { get; }
        object this[string key] { get;set; }
        object GetArgument(int index);
        object[] GetArguments();
        string ToString(IFormatProvider formatProvider);
    }

    public interface IMetaCollection<TUnit> : IMetaCollection where TUnit : class
    {
        IEnumerable<TEntity> EnumarateAs<TEntity>() where TEntity:class, IUnit<TUnit>;
    }

    public interface IMetaCollection
    {
        IEnumerable<KeyValuePair<string, object>> GetValues();
        IEnumerable<TUnit> BringTo<TUnit>() where TUnit : class;        
        IEnumerable<TSub> SupplyTo<TSub>() where TSub :class;
        IEnumerable<TUnit> Get<TUnit>(Predicate<TUnit> criteria = default) where TUnit :class;
    }
}
