using LinqToDB.Mapping;
using MissBot.Abstractions.Entities;
using MissBot.Common.Extensions;
using MissCore.Data.Collections;


[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
[Table("##BotUnits")]
public record BotUnit :BaseUnit, IBotUnit
{
    [Column()]
    public override string Entity { get; set; }
    [Column()]
    public virtual string Template { get; set; }
    [Column()]
    public string Description { get; set; }
    [Column()]
    public virtual string Payload { get; set; }    
    [Column()]
    public override string Unit { get ; set ; }
    [Column()]
    public virtual string Parameters { get; set; } = string.Empty;
    public override void InitializeMetaData()
        => Meta ??= MetaData.Parse(this);
}
