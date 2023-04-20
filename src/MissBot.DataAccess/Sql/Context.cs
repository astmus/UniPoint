using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using LinqToDB;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess.Interfacet;


namespace MissBot.DataAccess.Sql
{
    public record SQL<TUnit>(Func<TUnit, string> initFunc = default)
    {
        public TUnit Entity { get; init; } = Unit<TUnit>.Sample;
        public virtual string CreateCommand<TEntity>() where TEntity : TUnit
            => Request?.Command ?? initFunc(Entity);
        public SQL Request
            => request ?? (request =  (SQL)initFunc(Entity));
        SQL request;

        public static SQL<TUnit> Create(Func<TUnit, string> init)
             => new SQL<TUnit>(init);
        public record Query(Func<TUnit, string> initFunc) : SQLQuery<TUnit>(initFunc, default)
        {
            
        }
        
    }

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
                    result = JsonSerializer.Deserialize<TScalar>(res);
                    //result = des.Content.FirstOrDefault();
                    }
                    await connection.CloseAsync();
                }
                return result;
            }
            protected abstract string GetConnectionString();
        }

        public record SQLQuery<TEntity>(Func<TEntity, string> initFunc = default, TEntity Entity = default) : SQL<TEntity>(initFunc)
        {
            //public override SqlBuilder Request { get => base.Request; protected set => base.Request = value; }
            //public override SQL Request { get; protected set; } = new SQL(Command);
            //=> this with { Command = SqlQuery };

        }


    
}
