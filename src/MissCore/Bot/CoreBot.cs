using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissCore.Entities;
using Telegram.Bot.Types;
using static MissCore.Bot.BotCore;

namespace MissCore.Bot
{
    public abstract class BaseCoreBot : BaseBot
    {
        #region Context
        public class BotContext : LinqToDB.DataContext
        {
            public BotContext() { }
            internal BotContext(ContextOptions ctxOptions) : base(ctxOptions.driverName, ctxOptions.connectionString) { }
            internal record ContextOptions(string? connectionString, string? driverName, Func<DataOptions, DbConnection>? ConnectionFactory, Func<ConnectionOptions, IDataProvider>? DataProviderFactory);
            internal void Init()
            {
                Cmd<BotCommand>.Command = new BotCore.Cmd("SELECT * FROM ##BotCommands FOR JSON AUTO");
                Cmd<BotCommand>.Query = BotCore.RequestAny<BotCommand>("select * from ##BotCommands c INNER JOIN ##BotActionPayloads a ON c.Command = a.EntityAction where Command = '/{0}' for json path ");
                BotCore.SearchRequest.Request = new BotCore.SearchRequest("select * from ##BotActionPayloads where EntityAction = 'Bot.Search' FOR JSON PATH, WITHOUT_ARRAY_WRAPPER");
                Cmd<ActionPayload>.Command =  Cmd<ActionPayload>.Query with { cmd = ("Select * from ##BotActionPayloads where EntityAction = '{0}.{1}' FOR JSON PATH, WITHOUT_ARRAY_WRAPPER") };
            }
            static readonly internal ContextOptions BotContextOptions
                = new ContextOptions(null, null, null, null);
        }
        #endregion
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
            var data = Handle<ActionPayload>(BotCore.SearchRequest.Request);
            BotCore.SearchRequest.Request = BotCore.SearchRequest.Request with { Cmd = data.Payload };
        }
        public override async Task<bool> SyncCommands(IBotConnection connection)
        {
            Commands = await HandleAsync<List<BotCommand>>(Cmd<BotCommand>.Command, default);
            return await base.SyncCommands(connection);
        }

        protected async Task<TEntity> HandleAsync<TEntity>(BotCore.Cmd sql, CancellationToken cancellationToken) where TEntity : class
        {
            TEntity result = default(TEntity);
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = sql.cmd;

                var reader = await cmd.ExecuteReaderAsync(cancellationToken);
                if (!reader.HasRows)
                    return default(TEntity);
                else
                    while (await reader.ReadAsync())
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<TEntity>(reader.GetString(0));

            }
            return result;
        }
        protected TEntity Handle<TEntity>(BotCore.Cmd sql) where TEntity : class
        {
            TEntity result = default(TEntity);
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = sql.cmd;

                var reader = cmd.ExecuteReader();
                if (!reader.HasRows)
                    return default(TEntity);
                else
                    while (reader.Read())
                        result = Newtonsoft.Json.JsonConvert.DeserializeObject<TEntity>(reader.GetString(0));
                reader.Close();
            }
            return result;
        }
    }
}

