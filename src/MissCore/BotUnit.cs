using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using LinqToDB.Expressions;
using LinqToDB.Mapping;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using MissCore.Bot;
using MissCore.Collections;
using MissCore.Data;
using MissCore.Data.Entities;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissCore
{
    [Table("##BotUnits")]
    public record BotUnit<TUnit> : IBotUnit<TUnit> where TUnit : Unit
    {
        public static readonly (string Unit, string Entity) Key = new (Regex.Replace(typeof(TUnit).Name, "`1",""), typeof(TUnit).GetGenericArguments().FirstOrDefault()?.Name);
        public static readonly Id<TUnit> Id = new Id<TUnit>($"{Key.Unit}{Key.Entity}");
        public IEnumerable<IBotUnit> Units
            => Entities;
        public List<BotUnit> Entities { get; set; }

        [Column()]
        public virtual string Entity => Key.Entity;

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

        public IActionsSet GetUnitActions<TSub>(TSub unit) where TSub : Unit
        {
            return new UnitActions(Entities.Select(s
                 => UnitAction.WithCallbackData(s.Entity, string.Format(s.Template, s.Entity, s.Unit, unit.Meta.GetValue(s[0])))).ToList());
        }
    }
}

