using System.Data.Common;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.DataContext
{
    public interface IBotContext
    {
        void LoadBotInfrastructure();
        IEnumerable<BotCommand> BotCommands { get; }
        TCommand GetCommand<TCommand>() where TCommand : BotCommand, IBotUnitCommand;
        TUnit Get<TUnit>() where TUnit : class, IBotUnit;
        Task<IBotUnit<TUnit>> GetUnitAsync<TUnit>() where TUnit : Unit;
        IBotUnit<TUnit> GetUnit<TUnit>() where TUnit : Unit;
        TAction GetAction<TAction>() where TAction : class, IBotUnitCommand;
        Task<TAction> GetActionAsync<TAction>() where TAction : Unit, IBotUnitCommand;
        TUnit Get<TUnit, TEntity>() where TUnit :class, IBotUnit;
    }
    
}
