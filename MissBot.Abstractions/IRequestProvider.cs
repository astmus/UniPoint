using System.Linq.Expressions;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions
{
    public interface IRequestProvider
    {
        IUnitRequest<TUnit> ReadRequest<TUnit>(Expression<Predicate<TUnit>> criteria) where TUnit : Unit;
        IUnitRequest<TUnit> FindRequest<TUnit>(string search, uint skip = default, uint take = default) where TUnit : Unit;
        IUnitRequest<TUnit> FromRaw<TUnit>(string raw);
    }
}
