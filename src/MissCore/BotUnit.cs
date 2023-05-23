using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using LinqToDB.Expressions;
using LinqToDB.Mapping;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissCore.Bot;
using MissCore.Collections;
using MissCore.Data;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissCore
{
    [Table("##BotUnits")]
    public class BotUnit<TUnit> : IBotUnit<TUnit> where TUnit : Unit
    {
        public static readonly string Key = typeof(TUnit).Name;
        public IEnumerable<IBotUnit> Units
            => Entities;
        public List<BotUnit> Entities { get; set; }

        [Column()]
        public virtual string Entity => Key;

        public record Criteria(Expression left, ExpressionType operand, Expression right, CriteriaFormat Format = CriteriaFormat.SQL) : ICriteria
        {
            public string ToString(string? format, IFormatProvider? formatProvider) => operand switch
            {
                ExpressionType.Equal when left is MemberExpression p && right.Type == typeof(string)
                    => string.Format(format ?? Make(Format), p.Member.Name, " = ", $"'{right.EvaluateExpression()}'"),
                ExpressionType.Equal when left is MemberExpression p && right.Type != typeof(string)
                    => string.Format(format ?? Make(Format), p.Member.Name, " = ", right.EvaluateExpression()),
                //=> BinaryExpression.Equal(left, right).ToString(),
                _ => string.Format(format ?? Make(Format), left.ToString(), operand, right.EvaluateExpression())
            };
            public override string ToString()
            {
                return ToString(Make(Format), default);
            }
            static string Make(CriteriaFormat format)
            => format switch
            {
                CriteriaFormat.Unknown => $"Unknown format {format}",
                CriteriaFormat.SQL => " WHERE {0} {1} {2} ",
                _ => format.ToString()
            };
        }
        public class Collection : Unit<TUnit>.Collection { };
        public record Content : ContentUnit<TUnit>;
        public record Request<TData>(string Template) : UnitRequest<TData>(Template) where TData : class;

        public static Criteria CreateCriteria(Expression<Predicate<TUnit>> criteria)
        {
            if (criteria?.Body is BinaryExpression binexp)
                return new Criteria(binexp.Left, binexp.NodeType, binexp.Right);

            else return default;
        }

        //public static RequestInformation<TUnit> GetRequestInfo(Expression<Func<TUnit, object[]>> selector = default, Expression<Predicate<TUnit>> criteria = default)
        //{
        //    string[] fields = AllFileds;
        //    ICriteria icriteria = null;
        //    if (selector?.Body is NewArrayExpression exp)
        //        fields = (selector.Body as NewArrayExpression)?.Expressions.OfType<MemberExpression>().Select(s => s.Member.Name).ToArray();
        //    icriteria = CreateCriteria(criteria);
        //    return RequestInfo with { EntityFields = fields, Criteria = icriteria };
        //}        

        public IReplyMarkup GetUnitActions<TSub>(TSub unit) where TSub : Unit
        {
            return new InlineKeyboardMarkup(Entities.Select(s
                 => InlineKeyboardButton.WithCallbackData(s.Entity, string.Format(s.Template, s.Entity, s.Unit, unit.Meta.GetValue(s[0])))).ToList());
        }
    }
}

