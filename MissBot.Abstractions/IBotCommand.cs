
using MissBot.Abstractions.Actions;

namespace MissBot.Abstractions
{
    public interface IBotCommandData
    {
        string Payload { get; set; }        
    }

    public interface IInlineUnit
    {
        string Content { get; }
        int Id { get; }
        string Title { get;  }
        string Description { get; }
    }
    public interface IBotCommand : IBotAction
    {        
        string Description { get; }
    }
}
