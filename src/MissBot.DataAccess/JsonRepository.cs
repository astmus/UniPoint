using System.Data;
using System.Data.Common;
using System.Text;
using LinqToDB;
using Microsoft.Extensions.Options;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.DataAccess
{
    public class JsonRepository : DataContext, IJsonRepository
    {
        const string JSONAuto = " FOR JSON AUTO";
        const string JSONPath = " FOR JSON PATH";
        const string JSONRoot = ", ROOT('{0}')";
        public JsonRepository(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions.Value.DataProvider, ctxOptions.Value.ConnectionString)
        {

        }

        public Task ExecuteCommandAsync(IRepositoryCommand query, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public async Task<JArray> ReadUnitDataAsync<TUnit>(TUnit unit, CancellationToken cancel = default) where TUnit : IBotUnit
            => await HandleSqlAsync(unit.Payload, cancel);


        public Task<TResult> HandleCommandAsync<TResult>(IRepositoryCommand query, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public async Task<TResult> HandleQueryAsync<TResult>(IRepositoryCommand query, CancellationToken cancel = default) where TResult : class
        {
            TResult result = default(TResult);
            using (var conn = DataProvider.CreateConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = query.ToRequest() + JSONAuto;                      
                    var json = await ReadAsync(cmd, cancel);
                    result = JsonConvert.DeserializeObject<TResult>(json);
                }
            }
            return result;
        }

        private static async Task<string> ReadAsync(DbCommand cmd, CancellationToken cancel)
        {
            using (var reader = await cmd.ExecuteReaderAsync(CommandBehavior.SequentialAccess, cancel))
            {
                StringBuilder b = new StringBuilder();
                while (await reader.ReadAsync(cancel))
                    b.Append(await reader.GetTextReader(0).ReadToEndAsync(cancel));
                reader.Close();
                return b.ToString();
            }
        }

        public async Task<JArray> HandleReadAsync(IRepositoryCommand cmd, CancellationToken cancel = default)
        {                                                                    
            return await HandleSqlAsync(cmd.ToRequest(), cancel);
        }

        public async Task<JObject> HandleScalarAsync(IRepositoryCommand cmd, CancellationToken cancel = default)
        {
            StringBuilder result = new StringBuilder();
            await HandleAsync(cmd.ToRequest(), result, cancel);
            return JObject.Parse(result.ToString());
        }

        public async Task<ICollection<TResult>> HandleQueryResultAsync<TResult>(IRepositoryCommand cmd, CancellationToken cancel = default) where TResult : class
        {
            return await HandleQueryAsync<Unit<TResult>.Collection>(cmd, cancel);
        }

        protected virtual async Task<JArray> HandleSqlAsync(string sql, CancellationToken cancellationToken = default)
        {
            JArray result = default;
            using (var conn = DataProvider.CreateConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql + JSONAuto;
                    var json = await ReadAsync(cmd, cancellationToken);
                    result = JArray.Parse(json);
                }
                conn.Close();
            }
            return result;

        }

        protected virtual async Task HandleAsync(string sql, StringBuilder result, CancellationToken cancellationToken = default)
        {
            using (var connection = DataProvider.CreateConnection(ConnectionString))
            {
                await connection.OpenAsync(cancellationToken);
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                    if (!reader.HasRows)
                        reader.Close();
                    else
                    {
                        while (await reader.ReadAsync(cancellationToken))
                        {
                            result.Append(reader.GetString(0));
                            //object[] arr = new object[reader.FieldCount];
                            //reader.GetValues(arr);
                            //  result.AppendJoin(',', arr);
                        }
                        reader.Close();
                    }
                }

            }
            //return JsonConvert.DeserializeObject<TEntity>(result.ToString());
        }
    }
}

