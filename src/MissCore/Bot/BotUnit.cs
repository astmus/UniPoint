using LinqToDB.Mapping;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissCore;
using MissCore.Bot;

[JsonObject(MemberSerialization.OptOut, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
[Table("##BotUnits")]
public record BotUnit : IBotUnit
{
    public string this[int index]
        => Parameters?.Split(";").ElementAtOrDefault(index);

    [Column()]
    public virtual string Entity { get; set; }
    [Column()]
    public string Template { get; set; }
    [Column()]
    public string Description { get; set; }
    [Column()]
    public string Payload { get; set; }    
    [Column()]
    public virtual string Unit { get ; set ; }
    [Column()]
    public virtual string Parameters { get; set; }
    
    public virtual string Format(params object[] parameters)
    {
        return string.Format(Payload, parameters);
    }
}
