using LinqToDB.Mapping;
using MissBot.Abstractions.Actions;
using MissCore.Bot;

namespace MissCore.Data
{
	[Table("##BotUnits")]
	public record ExceptionUnit : DataUnit<Exception>
	{
		[Column("Unit")]
		public override string Unit { get; init; } = Unit<ExceptionUnit>.Key;

		[Column("Entity")]
		public override string Entity { get; set; }
	}
}
