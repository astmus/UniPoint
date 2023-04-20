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
using MissBot.DataAccess.Sql;
using MissCore.Entities;
using Telegram.Bot.Types;

namespace MissCore.Bot
{
    #region Context
    public class BotContext : LinqToDB.DataContext
    {
        public static SQL<TCommand> Command<TCommand>(TCommand cmd = default, SQLJson type = SQLJson.Path) where TCommand : BotCommand
                => new SQL<TCommand>(u
                => u is IBotCommand cmd ? $@"select * from ##BotCommands c
                INNER JOIN ##BotActionPayloads a ON c.Command = a.EntityAction where Command = '/{cmd.EntityAction.ToLower()}'" : throw new ArgumentException())
                ;

        public readonly static SQL<BotCommand> AllCommands
            = SQL<BotCommand>.Create(s =>  "SELECT * FROM ##BotCommands");
            

        public static SQL<Search> Search;
        public static SQL<ActionPayload> EntityActions;
        public BotContext() { }
        internal BotContext(ContextOptions ctxOptions) : base(ctxOptions.driverName, ctxOptions.connectionString) { }
        internal record ContextOptions(string? connectionString, string? driverName, Func<DataOptions, DbConnection>? ConnectionFactory, Func<ConnectionOptions, IDataProvider>? DataProviderFactory);
        internal void Init()
        {        
            
            Search = SQLQuery<Search>.Create( s => "select * from ##BotActionPayloads where EntityAction = 'Bot.Search'");
            EntityActions = SQLQuery<ActionPayload>.Create( e=> $"Select * from ##BotActionPayloads where EntityAction = '{e.EntityAction}.{e.Payload}' ");
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
            var unit = await HandleListAsync<BotCommand>(BotContext.AllCommands, default);
            Commands = unit.Content.ToArray();
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
                    result = JsonSerializer.Deserialize<Unit<TEntity>>(reader.GetString(0));
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
                        result = JsonSerializer.Deserialize<TEntity>(reader.GetString(0));

            }
            return result;
        }
        
    }
}

