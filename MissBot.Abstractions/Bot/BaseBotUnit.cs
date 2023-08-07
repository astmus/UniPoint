using System.Collections;
using System.Diagnostics;
using MissBot.Abstractions.Actions;
using MissBot.Entities.Abstractions;
using MissBot.Identity;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.Bot
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	public abstract record BaseBotUnit : BaseUnit, IBotUnit
	{
		public override object Identifier { get; set; } = nameof(BaseBotUnit);

		public override IEnumerator UnitEntities => Enumerable.Empty<IUnit>().GetEnumerator();

		[JsonProperty(Order = int.MinValue + 1)]
		public virtual string Entity { get; set; }
		public virtual string Description { get; set; }
		public abstract string Template { get; set; }
		public abstract string Format { get; set; }
		public abstract string Parameters { get; set; }
	}
	public abstract record BaseUnit<TData> : BaseUnit, IUnit
	{
	}
	//[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public abstract record BaseUnit : BaseItem, IUnit
	{
		[JsonProperty(Order = int.MinValue)]
		public virtual string Unit { get; set; } = nameof(BaseUnit);

		[JsonIgnore]
		public abstract IEnumerator UnitEntities { get; }

		public IEnumerator GetEnumerator()
			=> UnitEntities;

		private string GetDebuggerDisplay()
		{
			return string.Join(": ", this.Cast<JToken>().Select(s => s.Value<string>()).ToArray());
		}
	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	public abstract record BaseItem : IIdentibleUnit
	{
		public virtual object Identifier { get; set; } = Id<BaseItem>.Instance;
	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	public abstract record BaseAction : BaseItem, IBotAction
	{
		public override object Identifier
			=> Action;

		public virtual string Unit { get; set; }

		[JsonProperty("text", Required = Required.Always, Order = int.MinValue)]
		public virtual string Action { get; set; }

	}
}
