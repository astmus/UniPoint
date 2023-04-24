namespace MissBot.Abstractions.Entities
{
    public interface IBotUnit
    {
        string Command { get; set; }
        string Description { get; set; }
        string Payload { get; set; }
        string Placeholder { get; set; }
    }
}
