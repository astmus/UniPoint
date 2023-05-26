using System.Data.Common;
using LinqToDB;
using LinqToDB.Data;
using Microsoft.Extensions.Options;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissCore.Collections;
using MissCore.Data.Context;
using Newtonsoft.Json.Linq;

namespace MissCore.Bot
{

    public class BotContext<TBot> : BotContext, IBotContext<TBot> where TBot : IBot
    {
        public BotContext(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions)
        {
        }
    }
    public class BotContext : DataConnection, IBotContext
    {
        ITable<TUnit> GetUnits<TUnit>() where TUnit : class, IBotEntity
            => this.GetTable<TUnit>();
        ITable<TUnit> GetParameters<TUnit>() where TUnit : BotUnitParameter
        => this.GetTable<TUnit>();

        ReadUnit GetUnit;
        public IList<BotCommand> Commands { get; protected set; }
        public IList<BotUnitParameter> Parameters { get; protected set; }
        Lazy<Cache> lazyCache = new Lazy<Cache>();
        Cache cache => lazyCache.Value;
      

        public BotContext(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions.Value.DataProvider, ctxOptions.Value.ConnectionString)
        {
        }

        public void LoadBotInfrastructure()
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "Bot", "BotInit.sql"));
                cmd.ExecuteNonQuery();
                GetUnit = Get<ReadUnit>();
            }
            Commands = GetUnits<BotUnitCommand>().Where(w => w.Unit == Unit<BotCommand>.Key).Cast<BotCommand>().ToList();
            Parameters = GetParameters<UnitParameter>().Cast<BotUnitParameter>().ToList();
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
                    cmd.CommandText = query.GetCommand();
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

        public TUnit Get<TUnit>() where TUnit : Unit, IBotUnit
        {
            if (cache.Get(BotUnit<TUnit>.Id) is TUnit unit)
                return unit with { };

            unit = GetUnits<TUnit>().FirstOrDefault(w => w.Unit == BotUnit<TUnit>.Key.Unit);
            return cache.Set(unit, BotUnit<TUnit>.Id);
        }

        public TCommand GetCommand<TCommand>() where TCommand : BotCommand, IBotUnitCommand
        {
            if (cache.Get<TCommand>() is TCommand cmd)
                return cmd with { };

            cmd = GetUnits<TCommand>().FirstOrDefault(w => w.Unit == Unit<BotCommand>.Key && string.Compare(w.Action, Unit<TCommand>.Key, true) == 0);
            return cache.Set(cmd);
        }

        public TUnit Get<TUnit, TEntity>() where TUnit : class, IBotUnit
        {
            if (cache.Get<TUnit>(Id<TUnit, TEntity>.Value) is TUnit unit)
                return unit;

            unit = GetUnits<TUnit>().FirstOrDefault(w => w.Unit == Unit<TUnit>.Key && w.Entity == BotEntity<TEntity>.EntityKey);
            return unit;
        }

        public async Task<IBotUnit<TUnit>> GetUnitAsync<TUnit>() where TUnit : Unit
        {
            if (cache.Get(Id<BotUnit<TUnit>>.Value) is BotUnit<TUnit> unit)
                return unit with { };

            var cmd = GetUnit.Read<TUnit>();
            unit = await HandleQueryAsync<BotUnit<TUnit>>(cmd);
            return cache.Set(unit, Id<BotUnit<TUnit>>.Value);
        }

        IBotUnit<TUnit> IBotContext.GetUnit<TUnit>()
            => cache.Get<IBotUnit<TUnit>>(Id<IBotUnit, TUnit>.Value);

        TAction IBotContext.GetAction<TAction>()
            => cache.Get<TAction>(Id<TAction>.Value) with { };

        async Task<TAction> IBotContext.GetActionAsync<TAction>()
        {
            if (cache.Get<TAction>() is TAction action)
                return action with { };

            var cmd = GetUnit.Read<TAction>();
            action = await HandleQueryAsync<TAction>(cmd);
            return cache.Set(action);
        }
    }
}
