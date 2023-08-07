using System.Collections;
using LinqToDB.Mapping;
using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Abstractions;
using MissCore.Data;
using MissCore.Data.Collections;

namespace MissCore.Bot;

[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
[Table("##BotUnits")]
public record BotUnit : BaseBotUnit, IBotUnit
{
	[Column("Id"), PrimaryKey, NotNull]
	public override object Identifier => base.Identifier;

	[Column]
	public override string Unit { get; set; }

	[Column]
	public override string Entity { get; set; }


	[Column]
	public override string Format { get; set; }
	[Column]
	public override string Description { get; set; }

	[Column]
	public override string Template { get; set; }

	[Column]
	public override string Parameters { get; set; } = string.Empty;
	public override IEnumerator UnitEntities { get; }

	public override string ToString()
		=> Format;
}
