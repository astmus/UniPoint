using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.Actions
{
    public interface IBotUnitAction : IBotEntity
    {        
        string Action { get; }
    }

    public interface IBotUnitAction<TEntity> : IBotUnitAction,IBotUnit
    {
        Id<TEntity> UnitIdentifier { get; set; }        
    }
}
