using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LinqToDB;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess.Interfacet;
using Newtonsoft.Json;

namespace MissBot.DataAccess.Sql
{
    public record SQL<TUnit>(SQL Request = default, TUnit Entity = default)
    {
        public static implicit operator string(SQL<TUnit> cmd)
            => cmd.Request.Command;

        public virtual string CreateCommand<TEntity>() where TEntity : TUnit
            => Request.Command;


        public static SQL<TUnit> Create(string sql)
            => new SQL<TUnit>(new SQL() { Command = sql });/*, Activator.CreateInstance<TUnit>()*/

        //public static Query Initialize(SqlBuilder sqlfunc)
        //    => new Query(sqlfunc());

        public delegate SQL SqlBuilder(TUnit entity);
        public record Pager(int skip, int take, string search, Predicate<TUnit> filter = default) : SQL.Query<TUnit>;
        public record Query(SQL builder = default) : SQL.Query<TUnit>(builder, default)
        {
            
        }
        public SQL<TUnit> JsonAuto()
        {
            Request.Command = $"{CreateCommand<TUnit>()} FOR JSON AUTO";
            return this;
        }
        public SQL<TUnit> JsonPath(string wrapArray = ", WITHOUT_ARRAY_WRAPPER")
        {
            Request.Command = $"{CreateCommand<TUnit>()} FOR JSON PATH{wrapArray}";
            return this;
        }
        public SQL<TUnit> JsonItems()
        {
            Request.Command = $"select value from OpenJson(({Request.Command} FOR JSON AUTO))";
            return this;
        }
    }
    public  record SQL : Unit
    {
        public string Command { get; set; }
        
        public static implicit operator string(SQL cmd)
            => cmd.Command;

        public abstract class SQLContext : LinqToDB.DataContext, ISqlHandler
        {
            public SQLContext() :base(ProviderName.SqlServer2022, "")
            { }
       
            internal SQLContext(ContextOptions ctxOptions) : base(ctxOptions.driverName, ctxOptions.connectionString) { }
            internal record ContextOptions(string? connectionString, string? driverName = ProviderName.SqlServer2022);
            
            internal virtual ContextOptions BotContextOptions
                => new ContextOptions(GetConnectionString());
            public DbConnection OpenConnection(string sql, out DbCommand cmd)
            {
                var c = DataProvider.CreateConnection(GetConnectionString());
                cmd = c.CreateCommand();
                cmd.CommandText = sql;
                c.Open();
                return c;
            }
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
                        string res = (string)await cmd.ExecuteScalarAsync(cancel).ConfigureAwait(false);
                        result = JsonConvert.DeserializeObject<TScalar>(res);                    
                    }
                    await connection.CloseAsync();
                }
                return result;
            }
            protected abstract string GetConnectionString();
        }

        public record Query<TEntity>(SQL Sql = default, TEntity Entity = default) : SQL<TEntity>(Sql, Entity)
        {
            //public override SqlBuilder Request { get => base.Request; protected set => base.Request = value; }
            //public override SQL Request { get; protected set; } = new SQL(Command);
            //=> this with { Command = SqlQuery };

        }


    }
}
