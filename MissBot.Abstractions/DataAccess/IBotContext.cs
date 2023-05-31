using System.Data.Common;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissBot.Entities.Results.Inline;

namespace MissBot.Abstractions.DataAccess
{
    public interface IBotContext
    {
        IList<BotCommand> Commands { get; }
        IList<BotUnitParameter> Parameters { get; }
        TCommand GetCommand<TCommand>() where TCommand : BotCommand, IBotUnitAction;        
        TUnit Get<TUnit>() where TUnit : BaseUnit, IBotEntity;        
        IBotUnit<TUnit> GetBotUnit<TUnit>() where TUnit : BaseUnit;
        Task<IBotUnit<TUnit>> GetBotUnitAsync<TUnit>() where TUnit : BaseUnit;
        IEnumerable<IUnitAction<TUnit>> GetUnitActions<TUnit>() where TUnit : BaseUnit, IBotEntity;
        //IEnumerable<ResultUnit<InlineContent<TUnit>>> SearchResults<TUnit>(IEnumerable<TUnit> items, string query) where TUnit : BaseUnit, IBotEntity;
        TAction GetAction<TAction>() where TAction : BaseUnit, IBotUnitAction;
        Task<IBotUnitAction<TUnit>> GetActionAsync<TUnit>(string actionName) where TUnit : BaseUnit;        
    }

    public interface IBotContext<TBot> :IBotContext where TBot:IBot
    {
        void LoadBotInfrastructure();
    }
}
