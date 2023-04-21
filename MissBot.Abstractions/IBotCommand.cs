
namespace MissBot.Abstractions
{
    public interface IBotCommandData
    {
        string Payload { get; set; }
        string[] Params { get; set; }
    }

    public interface IBotCommand : IBotCommandData, IBotCommandInfo
    {

    }


    public interface IInlineUnit
    {
        string Content { get; }
        int Id { get; }
        string Title { get;  }
        string Description { get; }
    }
    public interface IBotCommandInfo
    {
        string Command { get; }
        string Description { get; }
    }
}
