
namespace MissBot.Abstractions
{
    public interface IBotCommandData
    {
        string Payload { get; set; }
        string[] Params { get; set; }
        string Name { get; set; }
    }

    public interface IBotCommandInfo
    {
        Type CmdType { get; }
        string Command { get; }
        string Description { get; }
    }
}
