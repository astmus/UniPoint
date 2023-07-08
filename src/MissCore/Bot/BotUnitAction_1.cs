using MissBot.Entities.Abstractions;
using MissBot.Identity;

namespace MissCore.Bot
{
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record BotAction<TEntity> : BotAction, IBotAction<TEntity>, IIdentibleUnit where TEntity : class
	{
		public override object Identifier => Id;
		public override string Template { get; set; }
		public Id<TEntity> Id { get; set; } = Id<TEntity>.Instance;
		public string Extension { get; set; }
	}
}
