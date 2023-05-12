using LinqToDB;
using LinqToDB.Common;
using Microsoft.Extensions.Options;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.DataModel;
using MissBot.DataAccess.Interfacet;

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
        public BotDataContext() : base(ProviderName.SqlServer2022, "")
        {
        }

        public BotDataContext(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions.Value.DataProvider, ctxOptions.Value.ConnectionString) { }

        //internal virtual BotContextOptions ContextOptions
        //    => new BotContextOptions(GetConnectionString());

        protected virtual string GetConnectionString() => "";

        public async Task<int> HandleRequestCommandAsync(string sql, CancellationToken cancel = default)
        {
            int result = 0;
            try
            {
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
            }
            catch (Exception error)
            {
                // sql.Result.AffectedRows = Convert.ToUInt32(result);
                //      sql.Result.ErrorCode = , error.HResult, error.Message);
            }
            return result;
        }
    }





}
