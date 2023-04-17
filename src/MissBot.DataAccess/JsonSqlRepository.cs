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
using MissBot.DataAccess.Sql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.DataAccess
{
    public class JsonSQLRepository : SqlRepository, IJsonRepository
    {
        public JsonSQLRepository(IConfiguration configuration) : base(configuration)
        {
        }

        protected async Task<TEntity> HandleAsync<TEntity>(string sql, IList<TEntity> isCollection, CancellationToken cancellationToken = default) where TEntity:class
        {
            //var str = await base.HandleAsync<TEntity>(sql, cancellationToken);
            var context = SQL.SQLContext.Init(ConnectionString);
            DbCommand cmd;
            TEntity result = default(TEntity);
            using (var conn = context.OpenConnection(sql, out cmd))
            {
                var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (!reader.HasRows)
                    return result;
                else
                {
                    if (isCollection == null)
                    {
                        await reader.ReadAsync();
                        var item = reader.GetString(0);
                        result = JsonConvert.DeserializeObject<TEntity>(item);
                    }
                    else
                        while (await reader.ReadAsync())
                        {
                            var item = reader.GetString(0);
                            isCollection.Add(JsonConvert.DeserializeObject<TEntity>(item));
                        }
                }
            }
            return result;
        }

        public Task ExecuteCommandAsync<TCommand>(TCommand sql, CancellationToken cancel = default) where TCommand:class
            => HandleAsync<TCommand>(JsonConvert.SerializeObject(sql), cancel);

        public Task<TResult> HandleQueryAsync<TResult>(string sql, CancellationToken cancel = default) where TResult : class
            => HandleAsync<TResult>(sql,null, cancel);

        public async Task<IList<TResult>> HandleQueryItemsAsync<TResult>(string sql, CancellationToken cancel) where TResult:class
        {
            var result = new List<TResult>();
            await HandleAsync<TResult>(sql, result, cancel);
            return result;
        }

        public async Task<JArray> HandleQueryGenericItemsAsync(string sql, CancellationToken cancel = default)
        {
            var array  = await base.HandleAsync<JArray>(sql, cancel);
            return array;
        }

        public async Task<JObject> HandleQueryGenericObjectAsync(string sql, CancellationToken cancel = default)
        {
            var obj = await base.HandleAsync<JObject>(sql, cancel);
            return obj;
        }
    }
}

