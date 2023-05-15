using System.Linq.Expressions;

namespace MissBot.Abstractions.DataAccess
{
    public interface IBotUnitFormatProvider : IFormatProvider
    {
        ICustomFormatter GetCriteriaFormatter();
        ICustomFormatter GetUnitFormatter();
    }
}
