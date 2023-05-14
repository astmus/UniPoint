namespace MissBot.Abstractions.Actions
{
    public interface IBotAction
    {
        string Command { get; set; }
        string CommandAction { get; }
    }

    public interface IBotAction<TEntity> : IBotAction, IBotCommandData
    {
        string Placeholder { get; set; }
        string Entity
            => typeof(TEntity).Name;

    }
}
