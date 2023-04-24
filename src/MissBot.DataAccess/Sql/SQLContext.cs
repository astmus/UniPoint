using LinqToDB;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess.Interfacet;

namespace MissBot.DataAccess.Sql
{
    public abstract class SQLDataContext : LinqToDB.DataContext, ISqlHandler
    {
        public SQLDataContext() : base(ProviderName.SqlServer2022, "")
        { }

        public SQLDataContext(ContextOptions ctxOptions) : base(ctxOptions.driverName, ctxOptions.connectionString) { }
        public record ContextOptions(string? connectionString, string? driverName = ProviderName.SqlServer2022);

        internal virtual ContextOptions BotContextOptions
            => new ContextOptions(GetConnectionString());
       
        protected abstract string GetConnectionString();

        public async Task<int> HandleSqlCommandAsync(ISQLUnit sql, CancellationToken cancel = default)
        {
            int result = 0;
            try
            {
                using (var connection = DataProvider.CreateConnection(GetConnectionString()))
                {
                    await connection.OpenAsync();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql.Command;
                        result = await cmd.ExecuteNonQueryAsync(cancel).ConfigureAwait(false);
                    }
                    await connection.CloseAsync();
                }
            }
            catch (Exception error)
            {
                sql.Result = new SQLResult(Convert.ToUInt32(result), error.HResult, error.Message);
            }
            return result;
        }        
    }

    



}
