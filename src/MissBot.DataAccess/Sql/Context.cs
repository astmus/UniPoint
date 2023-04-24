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
    //public delegate IEnumerable<string> FieldNamesSelector<TUnit>(TUnit entity);
    public record SQL<TUnit>(string rawSql = default) : SQLUnit
    {
        public TUnit Entity { get; init; } = Unit<TUnit>.Sample;
        public static readonly string EntityName = typeof(TUnit).Name;

        public virtual SQLUnit GetCommand()
            => this;
        public override SQLCommand Command
            => rawSql != null ? rawSql : SQL.Parse<TUnit>(Entity);
        SQLCommand request;

        public static SQL<TUnit> Create(FieldNamesSelector<TUnit> selector)
            => new Query(selector);

        public record Query(FieldNamesSelector<TUnit> selector) : SQL<TUnit>
        {
            IEnumerable<string> selectFields
                => selector(default(TUnit));

            public override SQLCommand Command
                => SQL.Entities<TUnit>(selector);
        }
        public record Query<TResult>(FieldNamesSelector<TResult> selector = default) : SQL<TUnit> where TResult: TUnit
        {
            IEnumerable<string> selectFields
                => selector(default(TResult));

            public static readonly Query<TResult> Instance = new Query<TResult>();
            public override SQLCommand Command
                => SQL.Entity<TResult>(selector);//  $"{SQL.SelectFromUnits} WHERE Entity = '{typeof(TUnit).Name}'".Replace("*", string.Join(',', selectFields));
        }
        public record Request : SQL<TUnit>
        {
            public override SQLCommand Command
                => $"{SQL.AllUnits} WHERE Entity = '{typeof(TUnit).Name}'";
        }
    }

    public abstract class SQLContext : SQLDataContext
    {
        public SQLContext() 
        { }

        //internal SQLContext(ContextOptions ctxOptions) : base(ctxOptions.driverName, ctxOptions.connectionString) { }
        //internal record ContextOptions(string? connectionString, string? driverName = ProviderName.SqlServer2022);

        //internal virtual ContextOptions BotContextOptions
        //    => new ContextOptions(GetConnectionString());
       
        public async Task ExecuteCommandAsync(SQLCommand sql, CancellationToken cancel = default)
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
        //public virtual async Task<int> HandleSqlQueryAsync(SQLCommand sql, CancellationToken cancel = default)
        //    => await HandleQueryAsync<TScalar>(sql.Command, cancel);

        public virtual async Task<TResult> HandleQueryAsync<TResult>(string sql, CancellationToken cancel = default) where TResult : class
        {
            TResult result = default(TResult);
            using (var connection = DataProvider.CreateConnection(GetConnectionString()))
            {
                await connection.OpenAsync();
                using (var cmd = connection.CreateCommand())
                {
                    cmd.CommandText = sql;
                    if (await cmd.ExecuteScalarAsync(cancel).ConfigureAwait(false) is string res)
                        result = JsonConvert.DeserializeObject<TResult>(res);
                }
                await connection.CloseAsync();
            }
            return result;
        }        
    }

    



}
