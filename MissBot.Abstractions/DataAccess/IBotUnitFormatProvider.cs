using System.Linq.Expressions;

namespace MissBot.Abstractions.DataContext
{
    public interface IBotUnitFormatProvider : IFormatProvider
    {
        ICustomFormatter GetCriteriaFormatter(IBotServicesProvider sp);
        ICustomFormatter GetUnitFormatter(IBotServicesProvider sp);
    }
}
