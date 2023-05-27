using System.Data.Common;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Entities;
using MissBot.Entities;

namespace MissBot.Abstractions.DataAccess
{
    public interface IBotContext
    {
        IList<BotCommand> Commands { get; }
        IList<BotUnitParameter> Parameters { get; }
        TCommand GetCommand<TCommand>() where TCommand : BotCommand, IBotUnitAction;        
        TUnit Get<TUnit>() where TUnit : UnitBase, IBotUnit;
        TUnit Get<TEntity, TUnit>(Id<TEntity> identifier) where TUnit : UnitBase, IBotEntity;
        IBotUnit<TUnit> GetBotUnit<TUnit>() where TUnit : UnitBase;
        Task<IBotUnit<TUnit>> GetBotUnitAsync<TUnit>() where TUnit : UnitBase;
        TAction GetAction<TAction>() where TAction : UnitBase, IBotUnitAction;
        Task<IBotUnitAction<TUnit>> GetActionAsync<TUnit>(string actionName) where TUnit : UnitBase;        
    }

    public interface IBotContext<TBot> :IBotContext where TBot:IBot
    {
        void LoadBotInfrastructure();
    }
}
