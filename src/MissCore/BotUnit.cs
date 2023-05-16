using System.Linq.Expressions;
using System.Reflection;
using LinqToDB.Expressions;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissCore.Bot;
using MissCore.Collections;

namespace MissCore
{

    public record RequestInformation<TEntity>(string Unit, string Entity, string[] EntityFields = null, ICriteria Criteria = null) : RequestInformation(Unit, Entity, EntityFields, Criteria);
    
    public static class BotUnit<TEntity> where TEntity : class
    {
        public record Criteria(Expression left, ExpressionType operand, Expression right, CriteriaFormat Format = CriteriaFormat.SQL) : ICriteria
        {
            public string ToString(string? format, IFormatProvider? formatProvider) => operand switch
            {
                ExpressionType.Equal when left is MemberExpression p && right.Type == typeof(string)
                    => string.Format(format, p.Member.Name, " = ", $"'{right.EvaluateExpression()}'"),
                ExpressionType.Equal when left is MemberExpression p && right.Type != typeof(string)
                    => string.Format(format, p.Member.Name, " = ", right.EvaluateExpression()),
                //=> BinaryExpression.Equal(left, right).ToString(),
                _ => string.Format(format, left.ToString(), operand, right.EvaluateExpression())
            };
        }

        public class Collection : Unit<TEntity>.Collection { };
        public record Content : ContentUnit<TEntity>;
        public record Request<TData> where TData : Unit<TEntity>;

        public static readonly RequestInformation<TEntity> RequestInfo
            = new($"{nameof(BotUnit)}s", Unit<TEntity>.Key);
        public static TEntityUnit Instance<TEntityUnit>() where TEntityUnit : Unit<TEntity>
            => Unit<TEntityUnit>.Sample;
         static readonly string[] AllFileds = { "*" };
        public static Criteria CreateCriteria(Expression<Predicate<TEntity>> criteria)
        {            
            if (criteria?.Body is BinaryExpression binexp)
                return new Criteria(binexp.Left, binexp.NodeType, binexp.Right);
            
            else return default;
        }
        public static RequestInformation<TEntity> GetRequestInfo(Expression<Func<TEntity, object[]>> selector = default, Expression<Predicate<TEntity>> criteria = default)
        {
            string[] fields = AllFileds;
            ICriteria icriteria = null;
            if (selector?.Body is NewArrayExpression exp)
                fields = (selector.Body as NewArrayExpression)?.Expressions.OfType<MemberExpression>().Select(s => s.Member.Name).ToArray();
            icriteria = CreateCriteria(criteria);
            return RequestInfo with { EntityFields = fields, Criteria = icriteria };
        }
    }
}

