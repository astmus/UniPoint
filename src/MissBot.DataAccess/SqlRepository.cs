using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB.Common.Internal;
using Microsoft.Extensions.Configuration;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess.Sql;
using Newtonsoft.Json.Linq;

namespace MissBot.DataAccess
{
    public class SqlRepository : ISqlRepository
    {
        private readonly IConfiguration config;
        protected string ConnectionString
            => config.GetConnectionString("Default");

        public SqlRepository(IConfiguration configuration)
        {
            config = configuration;
        }
        public async Task ExecuteCommandAsync(string sql, CancellationToken cancel = default)
        {
            await HandleAsync<string>(sql, cancel);
        }

        public async Task<string> HandleQueryAsync(string sql, CancellationToken cancel = default)
        {
            return (await HandleAsync<string>(sql, cancel)).ToString();
        }
        
        protected virtual async Task<TEntity> HandleAsync<TEntity>(string sql, CancellationToken cancellationToken = default) where TEntity:class
        {
            var context = SQL.SQLContext.Init(ConnectionString);
            DbCommand cmd;
            string result = "";
            JObject current = null;
            JArray all = new JArray();
            using (var conn = context.OpenConnection(sql, out cmd))
            {              
                var reader = await cmd.ExecuteReaderAsync(cancellationToken);

                if (!reader.HasRows)
                    return result as TEntity;
                else
                {
                    while (await reader.ReadAsync())
                    {
                        object[] arr = new object[reader.FieldCount];
                        
                        reader.GetValues(arr);
                        result = string.Join(',', arr);
                        current = JObject.Parse(result);
                        all.Add(current);
                    }                    
                }
            }
            return (all as TEntity) ?? (current as TEntity);
        }
    }
}

