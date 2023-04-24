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
       
        //public async Task ExecuteCommandAsync(SQL sql, CancellationToken cancel = default)
        //    => await ExecuteCommandAsync(sql.Command, cancel);

        //public async Task ExecuteCommandAsync(string sql, CancellationToken cancel = default)
        //{
        //    using (var connection = DataProvider.CreateConnection(GetConnectionString()))
        //    {
        //        await connection.OpenAsync();
        //        using (var cmd = connection.CreateCommand())
        //        {
        //            cmd.CommandText = sql;
        //            await cmd.ExecuteNonQueryAsync(cancel).ConfigureAwait(false);
        //        }
        //        await connection.CloseAsync();
        //    }
        //}
        //public virtual async Task<TScalar> HandleQueryAsync<TScalar>(SQL sql, CancellationToken cancel = default) where TScalar : class
        //    => await HandleQueryAsync<TScalar>(sql.Command, cancel);

        //public virtual async Task<TScalar> HandleQueryAsync<TScalar>(string sql, CancellationToken cancel = default) where TScalar : class
        //{
        //    TScalar result = default(TScalar);
        //    using (var connection = DataProvider.CreateConnection(GetConnectionString()))
        //    {
        //        await connection.OpenAsync();
        //        using (var cmd = connection.CreateCommand())
        //        {
        //            cmd.CommandText = sql;
        //            if (await cmd.ExecuteScalarAsync(cancel).ConfigureAwait(false) is string res)
        //                result = JsonConvert.DeserializeObject<TScalar>(res);
        //        }
        //        await connection.CloseAsync();
        //    }
        //    return result;
        //}
        protected abstract string GetConnectionString();

        public async Task<int> HandleSqlCommandAsync(SQLCommand sql, CancellationToken cancel = default)
        {
            int result = 0;
            using (var connection = DataProvider.CreateConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    result = await cmd.ExecuteNonQueryAsync(cancel).ConfigureAwait(false);
                }
                await connection.CloseAsync();
            }
            return result;
        }        
    }

    



}
