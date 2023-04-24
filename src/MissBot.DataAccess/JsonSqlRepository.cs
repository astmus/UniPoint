using System.Text;
using Microsoft.Extensions.Configuration;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using Newtonsoft.Json.Linq;

namespace MissBot.DataAccess
{
    public class JsonSqlRepository : SqlRepository, IJsonRepository
    {
        public JsonSqlRepository(IConfiguration configuration) : base(configuration)
        {
        }

       
        public override async Task<TResult> HandleQueryAsync<TResult>(ISQLUnit sql, CancellationToken cancel = default) where TResult : class
        {
            return await HandleSqlQueryAsync<TResult>(sql, cancel);            
        }

        public async Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(ISQLUnit sql, CancellationToken cancel) where TResult:class
        {            
            return await HandleSqlQueryAsync<Unit<TResult>.Collection>(sql, cancel);
        }

        public async Task<JArray> HandleQueryGenericItemsAsync(ISQLUnit sql, CancellationToken cancel = default)
        {
            StringBuilder result = new StringBuilder("[");
            await HandleAsync(sql.Command,  result, cancel);
            result.Append("]");
            return JArray.Parse(result.ToString());
        }

        public async Task<JObject> HandleQueryGenericObjectAsync(ISQLUnit sql, CancellationToken cancel = default)
        {
            StringBuilder result = new StringBuilder();
            await HandleAsync(sql.Command, result, cancel);
            return JObject.Parse(result.ToString());
        }

        protected virtual async Task HandleAsync(string sql, StringBuilder result, CancellationToken cancellationToken = default)
        {
            using (var conn = DataProvider.CreateConnection(GetConnectionString()))
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

