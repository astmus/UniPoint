using LinqToDB.Mapping;

using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.Abstractions;
using MissBot.Identity;

namespace MissCore.Bot
{
	[Table("##UnitActions")]
	public record BotAction : BaseBotAction, IBotAction
	{
		[Column]
		public virtual string Template { get; set; }

		[Column("Unit")]
		public override string Unit { get; set; }

		[Column]
		public string Parameters { get; set; }

		[Column]
		public override string Action { get; set; }
	}

	[Table("##BotUnits")]
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record BotUnitCommand : BotCommand, IExtendableUnit, IBotEntity
	{
		[Column("Unit")]
		public virtual string UnitKey { get; set; }

		[Column("Entity")]
		public override string EntityKey { get => Action; }

		[Column("Entity")]
		public override string Action { get; set; }

		[Column]
		public override string Description { get; set; }

		[Column]
		public override string Extension { get; set; }

		[Column]
		public virtual string Parameters { get; set; }

		[Column]
		public override string Template { get; set; }
	}

	[Table("##BotUnits")]
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record BotCommand<TEntity> : BotCommand
	{
		public override string EntityKey
			=> Id<TEntity>.Instance.Key;
	}
}
