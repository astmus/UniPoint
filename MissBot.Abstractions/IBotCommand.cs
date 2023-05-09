
using MissBot.Abstractions.Actions;

namespace MissBot.Abstractions
{
    public interface IBotCommandData
    {
        string Payload { get; set; }        
    }

    public interface IInlineUnit
    {
        object Content { get; }
        string Id { get; }
        string Title { get;  }
        string Description { get; }
    }
    public interface IBotCommand : IBotAction
    {        
        string Description { get; }
    }
}
