using MissBot.Abstractions.Entities;
using MissCore.Collections;

public record BotUnit : Unit, IBotUnit
{
    public virtual string Entity { get; }
    public string Command { get; set; }
    public string Placeholder { get; set; }
    public string Description { get; set; }
    public string Payload { get; set; }
}
