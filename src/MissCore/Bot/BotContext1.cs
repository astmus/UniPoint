using MissBot.DataAccess.Sql;
namespace MissCore.Bot
{
    public class BotContext : SQLDataContext
    {
        public static SQL<Search> Search;

        public BotContext() { }
        public BotContext(ContextOptions ctxOptions) : base(ctxOptions) { }
        //internal record ContextOptions(string? connectionString, string? driverName, Func<DataOptions, DbConnection>? ConnectionFactory, Func<ConnectionOptions, IDataProvider>? DataProviderFactory);
        internal virtual void Init()
        {
            Search = new SQL<Search>.Request();
        }

        protected override string GetConnectionString()
        {
            throw new NotImplementedException();
        }

        static readonly internal ContextOptions BotContextOptions
            = new ContextOptions(null, null);
    }
}

