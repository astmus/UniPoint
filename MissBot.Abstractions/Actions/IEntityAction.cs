namespace MissBot.Abstractions.Actions
{
    public interface IBotAction
    {
        string Entity { get; set; }
        string Payload { get; set; }
        string Placeholder { get; set; }
        string Command { get;  }
    }

    public interface IEntityAction<TEntity> : IBotAction
    {
     
    }
}
