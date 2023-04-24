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
using MissBot.DataAccess.Interfacet;
using MissBot.DataAccess.Sql;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissBot.DataAccess
{
    public class SqlRepository : SQLContext, ISqlRepository, ISqlHandler
    {
        private readonly IConfiguration config;

        protected override string GetConnectionString()
            => config.GetConnectionString("Default");     
        public SqlRepository(IConfiguration configuration)
            => config = configuration;  
       

        public async Task<TResult> HandleSqlQueryAsync<TResult>(ISQLUnit sql, CancellationToken cancel = default) where TResult:class
        {
            return await base.HandleQueryAsync<TResult>(sql, cancel);
        }

        public Task ExecuteCommandAsync(SQLUnit sql, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }
    }
}

