using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.Actions
{
    public interface IBotUnitCommand : IBotEntity
    {
        string Unit { get; }
        string Action { get; }
        string Command { get; }
    }

    public interface IUnitAction<TEntity> : IBotUnitCommand
    {
        string Template { get; set; }
        string Entity
            => typeof(TEntity).Name;
    }
}
