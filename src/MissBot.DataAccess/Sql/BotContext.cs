using System.Data;
using System.Data.Common;
using LinqToDB;
using LinqToDB.Common;
using LinqToDB.Data;
using Microsoft.Extensions.Options;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess.Interfacet;
using Newtonsoft.Json;

namespace MissBot.DataAccess.Sql
{

    public class BotContext : DataConnection, IBotContext
    {
        public BotContext() : base(ProviderName.SqlServer2022, "")
        {
        }


        public BotContext(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions.Value.DataProvider, ctxOptions.Value.ConnectionString)
        {     
        }

        public void LoadBotInfrastructure()
        {
            using (var cmd = Connection.CreateCommand())
            {
                cmd.CommandText = System.IO.File.ReadAllText(Path.Combine(Path.GetDirectoryName(Environment.ProcessPath), "Bot", "BotInit.sql"));
                cmd.ExecuteNonQuery();
            }
        }


        public async Task<int> HandleRequestCommandAsync(string sql, CancellationToken cancel = default)
        {
            int result = 0;
            try
            {
                using (var connection = DataProvider.CreateConnection(ConnectionString))
                {
                    await connection.OpenAsync();
                    using (var cmd = connection.CreateCommand())
                    {
                        cmd.CommandText = sql;
                        result = await cmd.ExecuteNonQueryAsync(cancel).ConfigureAwait(false);
                    }
                    await connection.CloseAsync();
                }
            }
            catch (Exception error)
            {
                // sql.Result.AffectedRows = Convert.ToUInt32(result);
                //      sql.Result.ErrorCode = , error.HResult, error.Message);
            }
            return result;
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

        public DbConnection NewConnection()
            => DataProvider.CreateConnection(ConnectionString);

    }





}
