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
    [Column("Unit")]
    public override string UnitKey { get; set; }

    [Column("Entity")]
    public override string EntityKey { get; set; }

    [Column]
    public virtual string Template { get; set; }

    [Column]
    public string Description { get; set; }

    [Column]
    public virtual string Extension { get; set; }

    [Column]
    public virtual string Parameters { get; set; } = string.Empty;

    public override string ToString()
        => Extension;
}
