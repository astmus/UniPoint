using System.Text;
using Microsoft.Extensions.Configuration;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using Newtonsoft.Json.Linq;

namespace MissBot.DataAccess
{
    public class JsonSqlRepository : BotRepository, IJsonRepository
    {
        public JsonSqlRepository(IConfiguration configuration) : base(configuration)
        {
        }       
        //public override async Task<TResult> HandleCommandAsync<TResult>(Abstractions.DataAccess.IBotCommandRequest query, CancellationToken cancel = default) 
        //{
        //    return await base.HandleCommandAsync<TResult>(query, cancel);            
        // }

        public async Task<JArray> HandleQueryGenericItemsAsync(BotRequest cmd, CancellationToken cancel = default)
        {
            StringBuilder result = new StringBuilder("[");
            await HandleAsync(cmd.Command,  result, cancel);
            result.Append("]");
            return JArray.Parse(result.ToString());
        }

        public async Task<JObject> HandleQueryGenericObjectAsync(BotRequest sql, CancellationToken cancel = default)
        {
            StringBuilder result = new StringBuilder();
            await HandleAsync(sql.Command, result, cancel);
            return JObject.Parse(result.ToString());
        }

        public async Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(BotRequest cmd, CancellationToken cancel = default) where TResult : class
        {
            return await base.HandleQueryAsync<Unit<TResult>.Collection>(cmd, cancel);
        }

        protected virtual async Task HandleAsync(string sql, StringBuilder result, CancellationToken cancellationToken = default)
        {
            using (var conn = Context.DataProvider.CreateConnection(GetConnectionString()))
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

