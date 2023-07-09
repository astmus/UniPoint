using System.Collections;
using System.Diagnostics;
using MissBot.Abstractions.Actions;
using MissBot.Entities.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.Bot
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	public abstract record BaseBotUnit : BaseItem, IBotEntity
	{
		public override object Identifier
			=> string.Join('.', Unit, Entity);

		[JsonProperty]
		public abstract string Unit { get; set; }

		[JsonProperty(Order = int.MinValue + 1)]
		public virtual string Entity { get; set; }
	}

	[DebuggerDisplay($"{{{nameof(GetDebuggerDisplay)}(),nq}}")]
	public abstract record BaseUnit : BaseItem, IUnit
	{
		[JsonProperty(Order = int.MinValue)]
		public virtual string Unit { get; set; }

		[JsonIgnore]
		public abstract IEnumerator UnitEntities { get; }

		public IEnumerator GetEnumerator()
			=> UnitEntities;

		public abstract void SetContext(object data);

		private string GetDebuggerDisplay()
		{
			return string.Join(": ", this.Cast<JToken>().Select(s => s.Value<string>()).ToArray());
		}
	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	public abstract record BaseItem : IIdentibleUnit
	{
		public abstract object Identifier { get; }
	}

	[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
	public abstract record BaseAction : BaseItem, IBotAction
	{
		public override object Identifier
			=> string.Join('.', Unit, Action);

		[JsonProperty]
		public virtual string Unit { get; set; }

		[JsonProperty("text", Required = Required.Always, Order = int.MinValue)]
		public virtual string Action { get; set; }

		public virtual string Entity
			=> Action;
	}
}
