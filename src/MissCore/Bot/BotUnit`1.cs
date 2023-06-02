using System.Collections.Specialized;
using System.Text.RegularExpressions;
using LinqToDB.Mapping;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Extensions.Entities;
using MissCore.Data.Collections;
using MissCore.Data.Entities;

namespace MissCore.Bot
{
    [Table("##BotUnits")]
    [JsonObject(MemberSerialization.OptOut)]
    public record BotUnit<TUnit> : BotUnit, IBotUnit<TUnit> where TUnit : BaseUnit
    {
        public static readonly (string Unit, string Entity) Key = new(Regex.Replace(typeof(TUnit).Name, "`1", ""), typeof(TUnit).GetGenericArguments().FirstOrDefault()?.Name);
        public static readonly Id<TUnit> Id = new Id<TUnit>($"{Key.Unit ?? nameof(BotUnit)}{Key.Entity}");
        public IEnumerable<IBotUnit> Units
            => Entities;
        public List<BotUnit> Entities { get; set; }

        [Column("Entity")]
        public override string EntityKey { get; set; } = Key.Entity;
        [Column("Unit")]
        public override string UnitKey { get; set; } = Key.Unit;        

        public record Content : ContentUnit<TUnit>;
        public void SetUnitActions<TSub>(TSub unit) where TSub : BaseUnit
        {
            IEnumerable<UnitAction> actions = Units.Select(s
                    => UnitAction.WithData(s.EntityKey, string.Format(s.Template, s.EntityKey, s.UnitKey, unit.Meta.GetValue(s[0])))).ToList();
            unit.Actions = new UnitActions(actions);
        }

        public override void InitializeMetaData()
        {
            Meta ??= MetaData<TUnit>.Parse(this);
        }
    }
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

//public record Criteria(Expression left, ExpressionType operand, Expression right, CriteriaFormat Format = CriteriaFormat.SQL) : ICriteria
//{
//    public string ToString(string? format, IFormatProvider? formatProvider) => operand switch
//    {
//        ExpressionType.Equal when left is MemberExpression p && right.Type == typeof(string)
//            => string.Format(format ?? Make(Format), p.Member.Name, " = ", $"'{right.EvaluateExpression()}'"),
//        ExpressionType.Equal when left is MemberExpression p && right.Type != typeof(string)
//            => string.Format(format ?? Make(Format), p.Member.Name, " = ", right.EvaluateExpression()),
//        //=> BinaryExpression.Equal(left, right).ToString(),
//        _ => string.Format(format ?? Make(Format), left.ToString(), operand, right.EvaluateExpression())
//    };
//    public override string ToString()
//    {
//        return ToString(Make(Format), default);
//    }
//    static string Make(CriteriaFormat format)
//    => format switch
//    {
//        CriteriaFormat.Unknown => $"Unknown format {format}",
//        CriteriaFormat.SQL => " WHERE {0} {1} {2} ",
//        _ => format.ToString()
//    };
//}
