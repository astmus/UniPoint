using System.Collections.Immutable;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Bot;
using MissBot.Entities.Abstractions;
using Microsoft.Extensions.DependencyInjection;
using MissCore.Data.Entities;

namespace MissBot.Abstractions.DataAccess
{

	public interface IBotContext
	{
		IList<BotCommand> Commands { get; }
		IImmutableList<IUnitParameter> Parameters { get; }
		IQueryUnit<TData> GetQueryUnit<TData>() where TData : class;
		TCommand GetCommand<TCommand>() where TCommand : BotCommand;
		TUnit GetUnit<TUnit>() where TUnit : BaseBotUnit;
		TEntity GetBotEntity<TEntity>() where TEntity : class, IBotEntity;
		IBotUnit<TUnit> GetBotUnit<TUnit>() where TUnit : BaseUnit;
		Task<IBotUnit<TUnit>> GetBotUnitAsync<TUnit>() where TUnit : class, IUnit;
		Task<TUnit> GetBotUnitAsync<TUnit, TEntity>() where TUnit : class, IBotUnit<TEntity> where TEntity : class, IUnit;
		IEnumerable<IBotUnit<TUnit>> SetUnitActions<TUnit>(IUnitCollection<TUnit> units) where TUnit : BaseUnit, IInteractableUnit;
		IQueryable<ResultUnit<TRes>> SearchResults<TRes>(IEnumerable<TRes> units) where TRes : BaseUnit;
		TAction GetAction<TAction>() where TAction : BaseBotUnit, IBotAction;
		Task<IUnitAction<TUnit>> GetUnitActionAsync<TUnit>(string actionName) where TUnit : class, IIdentibleUnit, IUnit;
		IInteractableUnit<TUnit> UnitActions<TUnit>(IInteractableUnit<TUnit> unit, byte rowCount = byte.MaxValue) where TUnit : class, IUnit;
	}

	public interface IBotContext<TBot> : IBotContext where TBot : IBot
	{
		void LoadBotInfrastructure();
	}
}
