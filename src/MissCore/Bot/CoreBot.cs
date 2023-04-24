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
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess.Sql;
using Newtonsoft.Json;
namespace MissCore.Bot
{
    #region Context
    public class BotContext : SQLDataContext
    {
        //public static SQL<TCommand> Command<TCommand>(TCommand cmd = default) where TCommand : BotCommand
        //        => new SQL<TCommand>(u
        //        => u is IBotCommand cmd ? $@"SELECT * FROM ##{nameof(BotCommand)}s  WHERE Command = '/{cmd.Command.ToLower()}'" : throw new ArgumentException())
        //        ;

        public readonly static SQL<BotCommand> AllCommands
            = new SQL<BotCommand>.Query(cmd => new[] { nameof(cmd.Command), nameof(cmd.Description) });


        public static SQL<Search> Search;

        public BotContext() { }
        public BotContext(ContextOptions ctxOptions) : base(ctxOptions) { }
        //internal record ContextOptions(string? connectionString, string? driverName, Func<DataOptions, DbConnection>? ConnectionFactory, Func<ConnectionOptions, IDataProvider>? DataProviderFactory);
        internal virtual void Init()
        {
            Search = new SQL<Search>.Request();// <DataBase> .Create(s => $"SELECT * FROM ##{nameof(BotUnit)}s WHERE Command = 'Search'");
        }

        protected override string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        static readonly internal ContextOptions BotContextOptions
            = new ContextOptions(null, null);
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
            Commands = await HandleListAsync<Unit<BotCommand>.Collection>(SQL.Entities<BotCommand>(c => new[] { nameof(c.Command), nameof(c.Description) }));            
            return await base.SyncCommands(connection);
        }
        protected async Task<TEntity> HandleListAsync<TEntity>(SQLCommand sql, CancellationToken cancellationToken = default) where TEntity : class
        {
            TEntity result = default(TEntity);
            using (var cmd = Connection.CreateCommand())
            {
                
                cmd.CommandText = sql.Command;

                var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (!reader.HasRows)
                    return result;
                else
                {
                    await reader.ReadAsync();
                    try
                    {
                        if (reader.GetString(0) is string str)
                            result = JsonConvert.DeserializeObject<TEntity>(str);

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
                cmd.CommandText = sql.Command.Command;

                var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (!reader.HasRows)
                    return default(TEntity);
                else
                    while (await reader.ReadAsync())
                        if (reader.GetString(0) is string str)
                            result = JsonConvert.DeserializeObject<TEntity>(str);

            }
            return result;
        }

    }
}

