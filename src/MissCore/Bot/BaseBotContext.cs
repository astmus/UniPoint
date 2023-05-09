using System.Data.Common;
using LinqToDB;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.DataModel;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess.Sql;
using Newtonsoft.Json;
namespace MissCore.Bot
{
    public abstract class BaseBotContext : BaseBot
    {
        DbConnection Connection;
        BotDataContext Context;
        public BaseBotContext( IRepository<BotCommand> commandsRepository = default) : base(commandsRepository)
        {
            
        }


        public override sealed void Init(IServiceProvider sp)
        {
            base.Init(sp);
            var config = sp.GetRequiredService<IConfiguration>();
            Context = sp.GetRequiredService<BotDataContext>();
            var Options = new { connectionString = config.GetConnectionString("Default"), driverName = ProviderName.SqlServer2022 };
            
            //Context.Init();
            Connection = Context.DataProvider.CreateConnection(Options.connectionString);
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

            Commands = await HandleAsync<Unit<BotCommand>.Collection>(MissBot.Abstractions.DataAccess.Unit.Entities<BotCommand>( c  => new[] { nameof(c.Command), nameof(c.Description) }));
            return await base.SyncCommands(connection);
        }
        protected async Task<TEntity> HandleAsync<TEntity>(SQLCommand sql, CancellationToken cancellationToken = default) where TEntity : class
        {
            TEntity result = default(TEntity);
    
                using (var cmd = Connection.CreateCommand())
                {
                    cmd.CommandText = sql.Command;
                    if (await cmd.ExecuteScalarAsync(cancellationToken) is string res)
                        result = JsonConvert.DeserializeObject<TEntity>(res);                    
                }
            
            return result;
        }
    }
}

