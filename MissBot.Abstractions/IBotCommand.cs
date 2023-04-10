
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
    }
    public interface IEntityAction<TEntity> : IEntityAction
    {
    }

    public interface IInlineUnit
    {
        object[] Content { get; }
        string Id { get; }
        string Title { get;  }
    }
    public interface IBotCommandInfo
    {
        Type CmdType { get; }
        string Command { get; }
        string Description { get; }
    }
}
