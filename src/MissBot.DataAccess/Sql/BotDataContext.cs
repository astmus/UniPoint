using System.Data;
using System.Data.Common;
using LinqToDB;
using LinqToDB.Common;
using Microsoft.Extensions.Options;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.DataModel;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess.Interfacet;
using Newtonsoft.Json;

namespace MissBot.DataAccess.Sql
{
    //public record BotContextOptions(string? connectionString, string? driverName = ProviderName.SqlServer2022);
    public class BotContextOptions
    {
        public const string ContextOptions = nameof(BotContextOptions);
        public string ConnectionString { get; set; } = String.Empty;
        public string DataProvider { get; set; } = String.Empty;
    }

    public class BotDataContext : LinqToDB.DataContext, IBotDataContext
    {

        public DbConnection Connection;
        public BotDataContext() : base(ProviderName.SqlServer2022, "")
        {
        }


        public BotDataContext(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions.Value.DataProvider, ctxOptions.Value.ConnectionString)
        {     
        }

        public void LoadBotInfrastructure()
        {
            Connection = DataProvider.CreateConnection(ConnectionString);
            KeepConnectionAlive = true;
            Connection.Open();
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

        public async Task<IEnumerable<BotCommand>> LoadCommandsAsync()
        {
            return await HandleAsync<Unit<BotCommand>.Collection>(MissBot.Abstractions.DataAccess.SqlUnit.Entities<BotCommand>(c => new[] { nameof(c.Command), nameof(c.Description) }));
        }

        public DbConnection NewConnection()
            => DataProvider.CreateConnection(ConnectionString);

    }





}
