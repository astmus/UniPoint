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
        TCommand GetCommand<TCommand>() where TCommand : BotCommand, IBotUnitCommand;        
        TUnit Get<TUnit>() where TUnit : Unit, IBotUnit;
        TUnit Get<TUnit, TEntity>() where TUnit : class, IBotUnit;
        IBotUnit<TUnit> GetUnit<TUnit>() where TUnit : Unit;
        Task<IBotUnit<TUnit>> GetUnitAsync<TUnit>() where TUnit : Unit;
        TAction GetAction<TAction>() where TAction : Unit, IBotUnitCommand;
        Task<TAction> GetActionAsync<TAction>() where TAction : Unit, IBotUnitCommand;        
    }

    public interface IBotContext<TBot> :IBotContext where TBot:IBot
    {
        void LoadBotInfrastructure();
    }
}
