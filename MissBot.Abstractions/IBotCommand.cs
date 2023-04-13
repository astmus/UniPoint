
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

    public interface IEntityAction
    {
        object? Id { get; }
        string Text { get; }
    }
    public interface IEntityAction<TEntity> : IEntityAction
    {
        
    }

    public interface IInlineUnit
    {
        string Content { get; }
        string Id { get; }
        string Title { get;  }
        string Description { get; }
    }
    public interface IBotCommandInfo
    {
        Type CmdType { get; }
        string Command { get; }
        string Description { get; }
    }
}
