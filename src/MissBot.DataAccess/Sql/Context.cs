using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess.Interfacet;
using Newtonsoft.Json;

namespace MissBot.DataAccess.Sql
{
    public record SQL<TUnit>(Func<TUnit, string> initFunc = default)
    {
        public TUnit Entity { get; init; } = Unit<TUnit>.Sample;

        public virtual SQL GetCommand()
            => SQLTemplate;
        public SQL SQLTemplate
            => !request.IsEmpty ? (request) : request = initFunc?.Invoke(Entity)  ?? SQL.Parse<TUnit>(Entity);
        SQL request;



        public static SQL<TUnit> Create(Func<TUnit, string> init) => new SQL<TUnit>(init);

        public record Query(Func<TUnit, string> initFunc = default) : SQL<TUnit>(initFunc)
        {
            
        }
        public record Query<TResult> : SQL<TUnit> where TResult: TUnit
        {

        }
        public record Request : SQL<TUnit>
        {

        }
    }

    public abstract class SQLContext : LinqToDB.DataContext, ISqlHandler
    {
        public SQLContext() : base(ProviderName.SqlServer2022, "")
        { }

        internal SQLContext(ContextOptions ctxOptions) : base(ctxOptions.driverName, ctxOptions.connectionString) { }
        internal record ContextOptions(string? connectionString, string? driverName = ProviderName.SqlServer2022);

        internal virtual ContextOptions BotContextOptions
            => new ContextOptions(GetConnectionString());
       
        public async Task ExecuteCommandAsync(SQL sql, CancellationToken cancel = default)
            => await ExecuteCommandAsync(sql.Command, cancel);

        public async Task ExecuteCommandAsync(string sql, CancellationToken cancel = default)
        {
            using (var connection = DataProvider.CreateConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    await cmd.ExecuteNonQueryAsync(cancel).ConfigureAwait(false);
                }
                await connection.CloseAsync();
            }
        }
        public virtual async Task<TScalar> HandleQueryAsync<TScalar>(SQL sql, CancellationToken cancel = default) where TScalar : class
            => await HandleQueryAsync<TScalar>(sql.Command, cancel);

        public virtual async Task<TScalar> HandleQueryAsync<TScalar>(string sql, CancellationToken cancel = default) where TScalar : class
        {
            TScalar result = default(TScalar);
            using (var connection = DataProvider.CreateConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (await cmd.ExecuteScalarAsync(cancel).ConfigureAwait(false) is string res)
                        result = JsonConvert.DeserializeObject<TScalar>(res);
                }
                await connection.CloseAsync();
            }
            return result;
        }
        protected abstract string GetConnectionString();
    }

    



}
