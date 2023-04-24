using System.Data.Common;
using LinqToDB;
using LinqToDB.Data;
using LinqToDB.DataProvider;
using Microsoft.Extensions.Configuration;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess.Sql;
using MissCore.Entities;

namespace MissCore.Bot
{
    public class BotContext<TBot> : BotContext where TBot:BaseBot
    {
        private readonly IConfiguration config;
        public BotContext(IConfiguration configuration)
            => config = configuration;  
        protected override string GetConnectionString()
            => config.GetConnectionString("Default");       
 
        //public static TCommand Command<TCommand>(TCommand cmd = default, SQLType type = SQLType.JSONPath) where TCommand : BotCommand
        //        => new BotUnit<TCommand>(u
        //        => u is IBotCommand cmd ? $@"SELECT * FROM ##{nameof(BotAction)}s  WHERE Command = '/{cmd.Command.ToLower()}'" : throw new ArgumentException())
        //        ;

        public readonly static SQL<BotAction>.Query<BotCommand> Commands
            = new  SQL<BotAction>.Query<BotCommand>(s=>new[] { nameof(s.CommandAction), nameof(s.Description) });


        public static SQL<Search> SearchRequest;

        public BotContext() { }
        //public BotContext(ContextOptions ctxOptions) : base(ctxOptions.driverName, ctxOptions.connectionString) { }
        public BotContext(ContextOptions ctxOptions) : base(ctxOptions) { }
        // internal record ContextOptions(string? connectionString, string? driverName, Func<DataOptions, DbConnection>? ConnectionFactory, Func<ConnectionOptions, IDataProvider>? DataProviderFactory);
        internal override void Init()
        {

            Search = new SQL<Search>($"SELECT * FROM ##{nameof(BotAction)}s WHERE Command = 'Search'");

        }

        //static readonly internal ContextOptions BotContextOptions
        //    = new ContextOptions(null, null, null, null);
    }
}

