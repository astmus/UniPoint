using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;

namespace MissBot.DataAccess.Sql
{
    public static class SQL
    {
        public class SQLContext : LinqToDB.DataContext
        {
            public SQLContext() { }
            public static SQLContext Instance { get; private set; }
            internal SQLContext(ContextOptions ctxOptions) : base(ctxOptions.driverName, ctxOptions.connectionString) { }
            internal record ContextOptions(string? connectionString, string? driverName = ProviderName.SqlServer2022);
            public static SQLContext Init(string connectionString)
            {
                //Bot.Request<BotCommand>.Query = new Bot.Sql("SELECT * FROM ##BotCommands FOR JSON AUTO");
                //Bot.Request<BotCommand>.Any = Bot.RequestForAnyType<BotCommand>("select * from ##BotCommands c INNER JOIN ##BotActionPayloads a ON c.Command = a.EntityAction where Command = '/{0}' for json path ");
                //Bot.SearchRequest.Request = new Bot.Sql("select * from ##BotActionPayloads where EntityAction = 'Search' FOR JSON PATH, WITHOUT_ARRAY_WRAPPER");
                //Bot.Request<PayloadBotCommand>.Query = new Bot.Sql("Select * from ##BotActionPayloads where EntityAction = '{0}' FOR JSON PATH, WITHOUT_ARRAY_WRAPPER");
                 return Instance ?? (Instance = new SQLContext(BotContextOptions with { connectionString = connectionString }));
            }
            static internal ContextOptions BotContextOptions
                = new ContextOptions(null);
            public DbConnection OpenConnection(string sql, out DbCommand cmd)
            {
                var c = DataProvider.CreateConnection(ConnectionString);
                cmd = c.CreateCommand();
                cmd.CommandText = sql;
                c.Open();
                return c;
            }
        }       

        public record Command(string Query = default)
        {

        }
        public record Query<TEntity>(string Query = default) : Command(Query)
        {
            public Query<TEntity> OfType<TInEntity>(params string[] args) where TInEntity : TEntity
                => this with { Query = string.Format(Query, args) };
        }

      
    }
}
