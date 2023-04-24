using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Common.Internal;
using LinqToDB.Linq.Builder;
using Microsoft.Extensions.Configuration;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.DataAccess
{
    public class JsonSqlRepository : SqlRepository, IJsonRepository
    {
        public JsonSqlRepository(IConfiguration configuration) : base(configuration)
        {
        }

       
        public async Task<TResult> HandleQueryAsync<TResult>(SQLUnit sql, CancellationToken cancel = default) where TResult : class
        {
            return await HandleSqlQueryAsync<TResult>(sql.Command, cancel);            
        }

        public async Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(SQLUnit sql, CancellationToken cancel) where TResult:class
        {            
            return await HandleSqlQueryAsync<Unit<TResult>.Collection>(sql.Command, cancel);
        }

        public async Task<JArray> HandleQueryGenericItemsAsync(SQLUnit sql, CancellationToken cancel = default)
        {
            StringBuilder result = new StringBuilder("[");
            await HandleAsync(sql.Command,  result, cancel);
            result.Append("]");
            return JArray.Parse(result.ToString());
        }

        public async Task<JObject> HandleQueryGenericObjectAsync(SQLUnit sql, CancellationToken cancel = default)
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

