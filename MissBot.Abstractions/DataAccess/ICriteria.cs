using System.Linq.Expressions;

namespace MissBot.Abstractions.DataAccess
{
    public interface ICriteria : IFormattable
    {
        Expression left { get; init; }
        ExpressionType operand { get; init; }
        Expression right { get; init; }
        CriteriaFormat Format { get; init; }
    }

    public enum CriteriaFormat
    {
        Unknown,
        SQL
    }
}
