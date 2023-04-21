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

       
        public override async Task<TResult> HandleQueryAsync<TResult>(SQL sql, CancellationToken cancel = default) where TResult : class
        {
            return await HandleScalarQueryAsync<TResult>(sql, cancel);            
        }

        public async Task<Unit<TResult>> HandleQueryItemsAsync<TResult>(SQL sql, CancellationToken cancel) where TResult:class
        {            
            return await HandleScalarQueryAsync<Unit<TResult>>(sql, cancel);
        }

        public async Task<JArray> HandleQueryGenericItemsAsync(SQL sql, CancellationToken cancel = default)
        {
            string raw = await HandleAsync(sql.Command, cancel);
            return JArray.Parse(raw);
        }

        public async Task<JObject> HandleQueryGenericObjectAsync(SQL sql, CancellationToken cancel = default)
        {
            string raw = await HandleAsync(sql.Command, cancel);
            return JObject.Parse(raw);
        }

        protected virtual async Task<string> HandleAsync(string sql, CancellationToken cancellationToken = default)
        {
            DbCommand cmd;
            StringBuilder result = new StringBuilder("[");

            using (var conn = OpenConnection(sql, out cmd))
            {
                var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (!reader.HasRows)
                    reader.Close();
                else
                {
                    while (await reader.ReadAsync())
                    {
                        object[] arr = new object[reader.FieldCount];
                        reader.GetValues(arr);
                        result.AppendJoin(',', arr);
                    }
                    reader.Close();
                }
            }
            result.Append("]");
            return result.ToString();
            //return JsonConvert.DeserializeObject<TEntity>(result.ToString());
        }
    }
}

