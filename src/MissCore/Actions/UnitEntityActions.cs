using MissBot.Entities.Abstractions;
using MissBot.Identity;
using MissCore.Bot;


namespace MissCore.Actions
{
	public record BotUnitAction<TUnit> : BotAction, IBotAction<TUnit> where TUnit : class
	{
		public Id<TUnit> Id { get; set; }
		public string Command { get; set; }
		public virtual string Extension { get; set; }

		public static implicit operator BotUnitAction<TUnit>(string data)
			=> JsonConvert.DeserializeObject<BotUnitAction<TUnit>>(data);
	}

	public record InlineEntityAction<TEntity> : BotUnitAction<TEntity>, IBotAction<TEntity> where TEntity : class
	{
		public string Text { get; set; }
		public virtual string Data
			=> string.Format(Action, Text);

		public static implicit operator InlineEntityAction<TEntity>(string data) =>
			new InlineEntityAction<TEntity>() { Action = data };
	}
}
