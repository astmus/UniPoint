using System.ComponentModel;
using System.Data.Common;
using System.Runtime.CompilerServices;
using System.Text.Json;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess.Sql;
using MissCore.Entities;
using Newtonsoft.Json;
namespace MissCore.Bot
{
    #region Context
    public class BotContext : LinqToDB.DataContext
    {
        public static SQL<TCommand> Command<TCommand>(TCommand cmd = default, SQLJson type = SQLJson.Path) where TCommand : BotCommand
                => new SQL<TCommand>(u
                => u is IBotCommand cmd ? $@"SELECT * FROM ##{nameof(BotAction)}s  WHERE Command = '/{cmd.Command.ToLower()}'" : throw new ArgumentException())
                ;

        public readonly static SQL<BotAction> AllCommands
            = SQL<BotAction>.Create(s =>  $"SELECT {nameof(s.Command)}, {nameof(s.Description)} FROM ##{nameof(BotAction)}s WHERE Entity = '{nameof(BotCommand)}'");
            

        public static SQL<Search> Search;
        public static SQL<BotAction> EntityActions;
        public BotContext() { }
        internal BotContext(ContextOptions ctxOptions) : base(ctxOptions.driverName, ctxOptions.connectionString) { }
        internal record ContextOptions(string? connectionString, string? driverName, Func<DataOptions, DbConnection>? ConnectionFactory, Func<ConnectionOptions, IDataProvider>? DataProviderFactory);
        internal void Init()
        {        
            
            Search = SQLQuery<Search>.Create( s => $"SELECT * FROM ##{nameof(BotAction)}s WHERE Command = 'Search'");
            EntityActions = SQLQuery<BotAction>.Create(
                e=> $"SELECT * FROM ##{nameof(BotAction)} WHERE 'Entity' = '{e.Entity}' AND Command = '{e.Command}' ");
        }       

            static readonly internal ContextOptions BotContextOptions
                = new ContextOptions(null, null, null, null);
    }
    #endregion
    public abstract class BaseCoreBot : BaseBot
    {
        DbConnection Connection;
        protected BotContext Context { get; set; }
        public BaseCoreBot(IRepository<BotCommand> commandsRepository = default) : base(commandsRepository) { }

        public override sealed void Init(IServiceProvider sp)
        {
            base.Init(sp);
            var config = sp.GetRequiredService<IConfiguration>();
            var options = BotContext.BotContextOptions with { connectionString = config.GetConnectionString("Default"), driverName = ProviderName.SqlServer2022 };
            Context = new BotContext(options);
            Context.Init();
            Connection = Context.DataProvider.CreateConnection(options.connectionString);
            Context.KeepConnectionAlive = true;

            Connection.Open();
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "Bot", "BotInit.sql"));
                cmd.ExecuteNonQuery();
            }
           // var data = HandleListAsync<BotCommand>(SQL.Query<BotCommand>.Sample);
            // BotCore.SearchRequest.Request = BotCore.SearchRequest.Request with { Cmd = data.Payload };
        }
        public override async Task<bool> SyncCommands(IBotConnection connection)
        {
            var unit = await HandleListAsync<BotAction>(BotContext.AllCommands, default);
            Commands = unit.Content.ToList();
            return await base.SyncCommands(connection);
        }
        protected async Task<Unit<TEntity>> HandleListAsync<TEntity>(SQL<TEntity> sql, CancellationToken cancellationToken = default) where TEntity : class
        {
            Unit<TEntity> result = default(Unit<TEntity>);
            using (var cmd = Connection.CreateCommand())
            {
                var c = sql.Request;
                cmd.CommandText = sql.Request.Command;

                var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (!reader.HasRows)
                    return result;
                else
                {
                    await reader.ReadAsync();
                    try
                    {
                    if (reader.GetString(0) is string str)
                        result = JsonConvert.DeserializeObject<Unit<TEntity>>(str);

                    }
                    catch (Exception e)
                    {

                        throw;
                    }
                }
                await reader.CloseAsync();
            }
            return result;
        }
        protected async Task<TEntity> HandleAsync<TEntity>(SQL<TEntity> sql, CancellationToken cancellationToken) where TEntity : class
        {
            TEntity result = default(TEntity);
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = sql.Request.Command;

                var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (!reader.HasRows)
                    return default(TEntity);
                else
                    while (await reader.ReadAsync())
                        result = System.Text.Json.JsonSerializer.Deserialize<TEntity>(reader.GetString(0));

            }
            return result;
        }
        
    }
}

