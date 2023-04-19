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
    public class JsonSqlRepository : SqlRepository, IJsonRepository
    {
        public JsonSqlRepository(IConfiguration configuration) : base(configuration)
        {
        }

        //protected async Task<TEntity> HandleAsync<TEntity>(string sql, IList<TEntity> isCollection, CancellationToken cancellationToken = default) where TEntity:class
        //{
        //    //var str = await base.HandleAsync<TEntity>(sql, cancellationToken);
            
        //    DbCommand cmd;
        //    TEntity result = default(TEntity);
        //    using (var connection = DataProvider.CreateConnection(GetConnectionString()))//  OpenConnection(sql, out cmd))
        //    {
        //        cmd = connection.CreateCommand();
        //        var reader = await cmd.ExecuteReaderAsync(cancellationToken);

        //        if (!reader.HasRows)
        //            return result;
        //        else
        //        {
        //            if (isCollection == null)
        //            {
        //                await reader.ReadAsync();
        //                var item = reader.GetString(0);
        //                result = JsonConvert.DeserializeObject<TEntity>(item);
        //            }
        //            else
        //                while (await reader.ReadAsync())
        //                {
        //                    var item = reader.GetString(0);
        //                    isCollection.Add(JsonConvert.DeserializeObject<TEntity>(item));
        //                }
        //        }
        //    }
        //    return result;
        //}

       
        public override async Task<TResult> HandleQueryAsync<TResult>(string sql, CancellationToken cancel = default) where TResult : class
        {
            return await HandleScalarQueryAsync<TResult>(sql, cancel);            
        }

        public async Task<IList<TResult>> HandleQueryItemsAsync<TResult>(string sql, CancellationToken cancel) where TResult:class
        {            
            return await HandleScalarQueryAsync<List<TResult>>(sql, cancel);
        }

        public async Task<JArray> HandleQueryGenericItemsAsync(string sql, CancellationToken cancel = default)
        {
            string raw = await HandleAsync(sql, cancel);
            return JArray.Parse(raw);
        }

        public async Task<JObject> HandleQueryGenericObjectAsync(string sql, CancellationToken cancel = default)
        {
            string raw = await HandleAsync(sql, cancel);
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

