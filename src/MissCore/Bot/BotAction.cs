using LinqToDB.Mapping;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Abstractions;

namespace MissCore.Bot
{
	[Table("##BotActions")]
	public record BotAction : BaseAction, IBotAction
	{
		[Column("Id"), PrimaryKey, NotNull]
		public override object Identifier => base.Identifier;
		[Column]
		public override string Unit { get; set; }

		[Column]
		public override string Action { get; set; }

		[Column]
		public virtual string Template { get; set; }

		[Column]
		public virtual string Parameters { get; set; }

	}

	[Table("##BotUnits")]
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record BotUnitCommand : BotCommand
	{
		[Column]
		public override string Unit { get; set; }

		[Column(nameof(Entity))]
		public override string Action { get; set; }

		[Column]
		public override string Description { get; set; }

		[Column]
		public override string Format { get; set; }

		[Column]
		public virtual string Parameters { get; set; }

		[Column]
		public override string Template { get; set; }
	}

	[Table("##BotUnits")]
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record BotCommand<TEntity> : BotCommand
	{

	}
}
