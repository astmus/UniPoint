using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.Actions
{
    public interface IBotUnitAction : IBotEntity
    {        
        string Action { get; }
        //string Command { get; }
        //string Payload { get; }
    }

    public interface IBotUnitAction<TEntity> : IBotUnitAction,IBotUnit
    {
        Id<TEntity> Identifier { get; set; }        
    }
}
