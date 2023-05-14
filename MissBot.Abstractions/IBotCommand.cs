
using MissBot.Abstractions.Actions;
using MissBot.Entities.Results.Inline;

namespace MissBot.Abstractions
{
    public interface IBotCommandData
    {
        string Payload { get; set; }
    }

    public interface IInlineUnit
    {
        public InlineQueryResultType Type { get; }
        object Content { get; }
        string Id { get; }
        string Title { get; }
        string Description { get; }
    }
    public interface IBotCommand : IBotAction
    {
        string Description { get; }
    }
}
