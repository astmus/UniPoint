using System.Data.Common;
using LinqToDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json;
namespace MissCore.Bot
{
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
        }
        public override async Task<bool> SyncCommands(IBotConnection connection)
        {
            Commands = await HandleAsync<Unit<BotCommand>.Collection>(SQL.Entities<BotCommand>(c => new[] { nameof(c.Command), nameof(c.Description) }));
            return await base.SyncCommands(connection);
        }
        protected async Task<TEntity> HandleAsync<TEntity>(ISQLUnit sql, CancellationToken cancellationToken = default) where TEntity : class
        {
            TEntity result = default(TEntity);
            try
            {
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = sql.Command;
                    if (await cmd.ExecuteScalarAsync(cancellationToken) is string res)
                        result = JsonConvert.DeserializeObject<TEntity>(res);                    
                }
            }
            catch (Exception e)
            {
                sql.Result = new SQLResult(0, e.HResult, e.Message);
            }
            return result;
        }
        //protected async Task<TEntity> HandleAsync<TEntity>(SQL<TEntity> sql, CancellationToken cancellationToken) where TEntity : class
        //{
        //    TEntity result = default(TEntity);
        //    using (var cmd = Connection.CreateCommand())
        //    {
        //        cmd.CommandText = sql.Command.Command;

        //        var reader = await cmd.ExecuteReaderAsync(cancellationToken);
        //        if (!reader.HasRows)
        //            return default(TEntity);
        //        else
        //            while (await reader.ReadAsync())
        //                if (reader.GetString(0) is string str)
        //                    result = JsonConvert.DeserializeObject<TEntity>(str);

        //    }
        //    return result;
        //}

    }
}

