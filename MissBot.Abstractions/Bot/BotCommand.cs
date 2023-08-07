using MissBot.Entities.Abstractions;

namespace MissBot.Abstractions.Bot
{
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	//[Table("##BotActions")]
	public record BotCommand : BaseAction, IBotCommand, ITemplatedUnit
	{
		//[Column]
		public override string Unit { get => base.Unit; set => base.Unit = value; }

		//[Column]
		public override string Action { get; set; }

		[JsonProperty("command")]
		public string Command => $"/{Action?.ToLowerInvariant()}";

		[JsonProperty("description")]
		public virtual string Description { get; set; }
		public virtual string Template { get; set; }
		public virtual string Format { get; set; }
		public string Entity
			=> Action;
	}
}
