using System.Collections.Immutable;
using System.Data.Common;

using LinqToDB;
using LinqToDB.Data;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities;
using MissBot.Entities.Abstractions;
using MissBot.Identity;

using MissCore.Actions;
using MissCore.Data;
using MissCore.Data.Context;
using MissCore.Data.Entities;
using MissCore.DataAccess;
using MissCore.Response;
using MissCore.Storage;

namespace MissCore.Bot
{

	public class BotContext<TBot> : BotContext, IBotContext<TBot> where TBot : IBot
	{
		public BotContext(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions)
		{
			//Db = db;
		}

		public BotDataContext Db { get; }
	}

	public class BotContext : DataConnection, IBotContext
	{
		ITable<TUnit> GetUnits<TUnit>() where TUnit : class, IBotEntity
			=> this.GetTable<TUnit>();

		IQueryable<UnitAction<TUnit>> GetActions<TUnit>(IInteractableUnit<TUnit> unit) where TUnit : class
			=> this.GetTable<UnitAction<TUnit>>().Where(w => w.Unit == unit.Unit);
		IQueryable<UnitAction<TUnit>> GetActions<TUnit>() where TUnit : class, IUnit
			=> this.GetTable<UnitAction<TUnit>>().Where(w => w.Unit == DataUnit<TUnit>.Key);
		IQueryable<BotAction<TUnit>> GetBotActions<TUnit>() where TUnit : class
		=> this.GetTable<BotAction<TUnit>>().Where(w => w.Unit == DataUnit<TUnit>.Key);

		BotQueryUnit<TUnit> QueryUnits<TUnit>() where TUnit : class
			=> this.GetTable<BotQueryUnit<TUnit>>().Where(w => w.Unit == "QueryUnit" && w.Entity == DataUnit<TUnit>.Key).FirstOrDefault();
		IQueryable<InlineResultUnit<TUnit>> GetResults<TUnit>() where TUnit : BaseUnit
			=> this.GetTable<InlineResultUnit<TUnit>>();

		ITable<TUnitParameter> GetParameters<TUnitParameter>() where TUnitParameter : UnitParameterBase
			=> this.GetTable<TUnitParameter>();

		ITable<BotCommandUnitRequest<TUnit>> GetRequest<TUnit>() where TUnit : class, IBotEntity
			=> this.GetTable<BotCommandUnitRequest<TUnit>>();

		ReadUnit _getBotUnit;
		//WithRequest _withUnit; 
		readonly Cache cache;
		public IList<BotCommand> Commands { get; protected set; }
		public IImmutableList<IUnitParameter> Parameters { get; protected set; }

		public BotContext(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions.Value.DataProvider, ctxOptions.Value.ConnectionString)
		{
			cache = new Cache();
			Setup();
		}

		public void Setup()
		{
			LinqToDB.Data.DataConnection.TurnTraceSwitchOn();
			LinqToDB.Data.DataConnection.WriteTraceLine = (message, displayName, level)
				=>
			{ Console.WriteLine($"{message} {displayName}"); };
		}
		public void LoadBotInfrastructure()
		{
			using (var cmd = Connection.CreateCommand())
			{
				this.CreateTable<BotUnit>(tableOptions: TableOptions.CheckExistence);
				this.CreateTable<BotAction>(tableOptions: TableOptions.CheckExistence);
				this.CreateTable<UnitParameter>(tableOptions: TableOptions.CheckExistence);
				this.CreateTable<BotUnitParameter>(tableOptions: TableOptions.CheckExistence);
				this.CreateTable<ResultUnit<BotAction>>(tableOptions: TableOptions.CheckExistence);
				this.CreateTable<ResultUnit<Chat>>(tableOptions: TableOptions.CheckExistence);
				this.CreateTable<ResultUnit<User>>(tableOptions: TableOptions.CheckExistence);
				cmd.CommandText = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "Bot", "BotInit.sql"));
				cmd.ExecuteNonQuery();
			}
			_getBotUnit = GetBotEntity<ReadUnit>();
			//_withUnit = GetBotEntity<WithRequest>();
			Commands = GetUnits<BotUnitCommand>().Where(w => w.Unit == DataUnit<BotCommand>.Key).Cast<BotCommand>().ToList();
			Parameters = GetParameters<BotUnitParameter>().Cast<IUnitParameter>().ToImmutableList();
		}

		public async Task<TResult> HandleQueryAsync<TResult>(IUnitRequest query, CancellationToken cancel = default) where TResult : class
			=> await HandleCommandAsync<TResult>(query, cancel);

		public async Task<TResult> HandleCommandAsync<TResult>(IUnitRequest query, CancellationToken cancel = default)
		{
			var result = default(TResult);
			using (var connection = CreateConnection())
			{
				await connection.OpenAsync(cancel);
				using (var cmd = connection.CreateCommand())
				{
					cmd.CommandText = query.Get();
					try
					{
						if (await cmd.ExecuteScalarAsync(cancel).ConfigureAwait(false) is string res)
							result = JsonConvert.DeserializeObject<TResult>(res);
					}
					finally
					{
						await connection.CloseAsync();
					}
				}
			}
			return result;
		}

		DbConnection CreateConnection()
			=> DataProvider.CreateConnection(ConnectionString);

		public TUnit GetUnit<TUnit>() where TUnit : BaseBotUnit
		{
			if (cache.Get(Id<TUnit>.Instance) is TUnit unit)
				return unit;

			unit = GetUnits<TUnit>().FirstOrDefault(w
				=> w.Unit == BotUnit<TUnit>.Key.Unit);

			return cache.Set(unit, Id<TUnit>.Instance);
		}

		public IQueryUnit<TData> GetQueryUnit<TData>() where TData : class
			=> QueryUnits<TData>();

		public TCommand GetCommand<TCommand>() where TCommand : BotCommand
		{
			if (cache.Get<TCommand>() is TCommand cmd)
				return cmd with { };

			cmd = GetUnits<TCommand>().FirstOrDefault(w
				=> w.Unit == BotUnit<BotCommand>.Key.Unit && w.Action == DataUnit<TCommand>.Key);

			return cache.Set(cmd);
		}

		public async Task<IBotUnit<TUnit>> GetBotUnitAsync<TUnit>() where TUnit : class, IUnit
		{
			if (cache.Get<BotUnit<TUnit>>(BotUnit<TUnit>.UnitId) is BotUnit<TUnit> unit)
				return unit with { };

			var cmd = _getBotUnit.Read<TUnit>();
			unit = await HandleQueryAsync<BotUnit<TUnit>>(cmd);

			return cache.Set(unit, BotUnit<TUnit>.UnitId);
		}

		IBotUnit<TUnit> IBotContext.GetBotUnit<TUnit>()
			=> cache.Get<IBotUnit<TUnit>>(Id<IBotUnit, TUnit>.Instance);

		TAction IBotContext.GetAction<TAction>()
			=> cache.Get<TAction>(Id<TAction>.Instance) with { };

		public async Task<IBotAction<TUnit>> GetActionAsync<TUnit>(string actionName) where TUnit : class, IIdentibleUnit
		{
			if (cache.Get<BotAction<TUnit>>(actionName) is BotAction<TUnit> action)
				return action;

			action = await GetBotActions<TUnit>().FirstOrDefaultAsync(action
				=> string.Compare(action.Action, actionName, true) == 0);

			return cache.Set(action, actionName);
		}

		//public IUnitActions<TUnit> GetUnitActions<TUnit>() where TUnit : class, IUnitEntity, IIdentible
		//{
		//    if (cache.Get<UnitActions<TUnit>>() is UnitActions<TUnit> actions)
		//        return actions;

		//    actions = new UnitActions<TUnit>(GetActions<TUnit>().ToArray());

		//    return cache.Set(actions);
		//}

		public TEntity GetBotEntity<TEntity>() where TEntity : class, IBotEntity
		{
			if (cache.Get(Id<TEntity>.Instance) is TEntity entity)
				return entity;

			entity = GetUnits<TEntity>().FirstOrDefault(w =>
			   w.Entity == DataUnit<TEntity>.Key);

			return cache.Set(entity, Id<TEntity>.Instance);
		}


		public IEnumerable<IBotUnit<TUnit>> SetUnitActions<TUnit>(IUnitCollection<TUnit> units) where TUnit : BaseUnit, IInteractableUnit
		{
			var res = from resUnit in units
					  join actions in GetActions<TUnit>() on resUnit.Unit equals actions.Unit into actionUnits
					  from actionUnit in actionUnits.DefaultIfEmpty()
						  // group actionUnit.Identifier
					  select actionUnit;
			foreach (var a in res)
			{
				yield return a as BotUnit<TUnit>;
			}

		}

		public IQueryable<ResultUnit<TUnit>> SearchResults<TUnit>(IEnumerable<TUnit> units) where TUnit : BaseUnit
		{
			var query = from resUnit in units
						join results in GetResults<TUnit>() on resUnit.Unit equals results.Unit into resultUnits
						//from resultUnit in resultUnits.DefaultIfEmpty()
						select resUnit;
			return default;//query.As .AsQueryable<ResultUnit<TUnit>>();
		}


		async Task<TUnit> IBotContext.GetBotUnitAsync<TUnit, TEntity>()
		{
			if (cache.Get<TUnit>(BotUnit<TEntity>.UnitId) is TUnit unit)
				return unit;

			var cmd = _getBotUnit.Read<TEntity>();
			unit = await HandleQueryAsync<TUnit>(cmd);

			return cache.Set(unit, BotUnit<TEntity>.UnitId);
		}

		public IInteractableUnit<TUnit> UnitActions<TUnit>(IInteractableUnit<TUnit> unit, byte rowCount = byte.MaxValue) where TUnit : class, IUnit
		{
			var actions = GetActions(unit).Select(s => s.WithUnitContextId(unit)).ToList();
			unit.Actions = actions.Chunk(rowCount).ToArray();
			return unit;
		}

		public async Task<IUnitAction<TUnit>> GetUnitActionAsync<TUnit>(string actionName) where TUnit : class, IIdentibleUnit, IUnit
		{
			if (cache.Get<UnitAction<TUnit>>(actionName) is UnitAction<TUnit> action)
				return action;

			action = await GetActions<TUnit>().FirstOrDefaultAsync(action
				=> string.Compare(action.Action, actionName, true) == 0);

			return cache.Set(action, actionName);
		}

	}
}
