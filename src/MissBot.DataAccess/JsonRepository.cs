using System.Text;
using LinqToDB;
using Microsoft.Extensions.Options;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess.Sql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.DataAccess
{
    public class JsonRepository : DataContext, IJsonRepository
    {
        public JsonRepository(IOptions<BotContextOptions> ctxOptions) : base(ctxOptions.Value.DataProvider, ctxOptions.Value.ConnectionString)
        {
        
        }

        public Task ExecuteCommandAsync(IRepositoryCommand query, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

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
                    cmd.CommandText = query.Command;
                    if (await cmd.ExecuteScalarAsync(cancel) is string res)
                        result = JsonConvert.DeserializeObject<TResult>(res);
                }
            }
            return result;
        }

        public async Task<JArray> HandleQueryGenericItemsAsync(IRepositoryCommand cmd, CancellationToken cancel = default)
        {
            StringBuilder result = new StringBuilder("[");
            await HandleAsync(cmd.Command,  result, cancel);
            result.Append("]");
            return JArray.Parse(result.ToString());
        }

        public async Task<JObject> HandleQueryGenericObjectAsync(IRepositoryCommand sql, CancellationToken cancel = default)
        {
            StringBuilder result = new StringBuilder();
            await HandleAsync(sql.Command, result, cancel);
            return JObject.Parse(result.ToString());
        }

        public async Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(IRepositoryCommand cmd, CancellationToken cancel = default) where TResult : class
        {
            return await HandleQueryAsync<Unit<TResult>.Collection>(cmd, cancel);
        }

        protected virtual async Task HandleAsync(string sql, StringBuilder result, CancellationToken cancellationToken = default)
        {
            using (var conn = DataProvider.CreateConnection(ConnectionString))
            {
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                    if (!reader.HasRows)
                        reader.Close();
                    else
                    {
                        while (await reader.ReadAsync())
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

