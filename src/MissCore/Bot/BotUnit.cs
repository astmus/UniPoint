using LinqToDB.Mapping;
using MissBot.Abstractions.Entities;
using MissCore;
using MissCore.Bot;

[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
[Table("##BotUnits")]
public record BotUnit :UnitBase, IBotUnit
{
    [Column()]
    public override string Entity { get; set; }
    [Column()]
    public string Template { get; set; }
    [Column()]
    public string Description { get; set; }
    [Column()]
    public virtual string Payload { get; set; }    
    [Column()]
    public virtual string Unit { get ; set ; }
    [Column()]
    public virtual string Parameters { get; set; }    
}
