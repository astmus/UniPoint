namespace MissBot.Abstractions.Actions;


public interface IUnitAction
{
    string Unit { get; set; }
    string ActionName { get; set; }
}

public interface IUnitAction<TEntity> : IUnitAction where TEntity : class
{
    string Template { get; set; }
}
