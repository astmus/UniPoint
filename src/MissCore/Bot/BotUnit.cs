using LinqToDB.Mapping;
using MissBot.Abstractions.Entities;
using MissBot.Common.Extensions;
using MissCore.Data.Collections;


[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
[Table("##BotUnits")]
public record BotUnit : BaseUnit, IBotUnit
{
    public override object Identifier
        => $"{UnitKey}.{EntityKey}";

    [JsonProperty("Unit")]
    [Column("Unit")]
    public override string UnitKey { get ; set ; }

    [JsonProperty("Entity")]
    [Column("Entity")]
    public override string EntityKey { get; set; }

    [JsonProperty]
    [Column]
    public virtual string Template { get; set; }

    [JsonProperty]
    [Column]
    public string Description { get; set; }

    [JsonProperty]
    [Column]
    public virtual string Payload { get; set; }

    [JsonProperty]
    [Column]
    public virtual string Parameters { get; set; } = string.Empty;
    public override void InitializeMetaData()
        => Meta ??= MetaData.Parse(this);
}
