using System.Linq.Expressions;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions
{
    public abstract record RequestInformation(string Unit, string Entity, string[] EntityFields = default, ICriteria Criteria = default) : IRequestInformation;
    public interface IRequestInformation
    {
        ICriteria Criteria { get; init; }
        string Entity { get; init; }
        string[] EntityFields { get; init; }
        string Unit { get; init; }
    }
    public interface IRequestProvider
    {
        IUnitRequest Request<TUnit>(RequestInformation info = default) where TUnit : class;
        IUnitRequest RequestUnit<TUnit>(RequestInformation info = default) where TUnit : class;
        RequestInformation Info<TUnit>(Expression<Predicate<TUnit>> criteria) where TUnit : class;
        IUnitRequest RequestByCriteria<TUnit>(Expression<Predicate<TUnit>> criteria) where TUnit : class;
    }
}
