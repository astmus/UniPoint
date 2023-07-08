using System.Collections;
using System.Text.RegularExpressions;

using LinqToDB.Mapping;

using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Abstractions;
using MissBot.Identity;
using Newtonsoft.Json.Linq;

namespace MissCore.Bot
{
	[Table("##BotUnits")]
	[JsonObject(MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore)]
	public record BotUnit<TData> : BaseBotUnit, IBotUnit<TData>, IInteractableUnit<TData>, IParameterizedUnit where TData : class, IIdentibleUnit
	{
		public static readonly (string Unit, string Entity) Key = new(Regex.Replace(typeof(TData).Name, "`1", ""), typeof(TData).GetGenericArguments().FirstOrDefault()?.Name);
		public static readonly Id<TData> UnitId = Id<TData>.Join(Key.Unit, Key.Entity);

		[Column("Entity")]
		public override string EntityKey { get; set; } = Key.Entity;

		[Column("Unit")]
		public override string UnitKey { get; set; } = Key.Unit;

		[Column]
		public string Parameters { get; set; }

		[Column]
		public string Extension { get; set; }

		public IUnitContext DataContext { get; set; }

		public IEnumerable<IEnumerable<IUnitAction<TData>>> Actions { get; set; }
		public TData UnitData
			=> DataContext.GetUnitEntity<TData>();

		public IEnumerator UnitEntities
			=> DataContext?.UnitEntities;

		public void SetContext<TDataUnit>(TDataUnit data) where TDataUnit : class, IUnit<TData>
		{
			throw new NotImplementedException();
		}

		public void SetContext(object data)
		{
			throw new NotImplementedException();
		}

		public void SetContextRoot<TRoot>(TRoot data) where TRoot : JToken
		{
			throw new NotImplementedException();
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
