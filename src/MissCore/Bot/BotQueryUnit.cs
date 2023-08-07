using System.Collections;
using System.Text.RegularExpressions;

using LinqToDB.Mapping;

using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities.Abstractions;
using MissBot.Identity;
using MissCore.Actions;
using MissCore.Data;
using MissCore.DataAccess;
using Newtonsoft.Json.Linq;

namespace MissCore.Bot
{
	[Table("##BotUnits")]
	[JsonObject(MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore)]
	public class BotQueryUnit<TData> : FormattableUnit, IQueryUnit<TData>, IParameterizedUnit, ITemplatedUnit where TData : class
	{
		public static readonly (string Unit, string Entity) Key = new(Regex.Replace(typeof(TData).Name, "`1", ""), typeof(TData).GetGenericArguments().FirstOrDefault()?.Name);
		public static readonly Id<TData> UnitId = Id<TData>.Join(Key.Unit, Key.Entity);
		public BotQueryUnit()
		{
		}
		internal BotQueryUnit(string format) : base(format)
		{
		}

		internal BotQueryUnit(string format, IEnumerable<object> args = null) : base(format, args)
		{
		}

		internal BotQueryUnit(string format, IEnumerable<string> parameters = null) : base(format, parameters)
		{
		}

		[Column]
		public virtual string Entity { get; set; } = Key.Entity;

		[Column]
		public virtual string Unit { get; set; } = Key.Unit;

		[Column]
		public virtual string Parameters { get; set; }

		[Column]
		public virtual string Template { get; set; }

		public override string Format
			=> string.Format(Template, Parameters) + Options;

		public IEnumerable<IUnitParameter> Params { get; }

		public string Options { get; set; }

		public string Get(params object[] args)
		{
			return string.Format(Template + Options, args);
		}
		//public IUnitContext DataContext { get; set; }	
	}
}
