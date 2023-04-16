using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Common;

namespace MissCore.Bot
{
    static public class Bot
    {
        public record Sql(string Query = default)
        {
            protected virtual string CreateSpecific(params string[] args)
                => string.Format(Query, args);
        }
        public record SqlAny<TEntity>(string Query = default) : Sql(Query)
        {
            public SqlAny<TEntity> OfType<TInEntity>(params string[] args) where TInEntity : TEntity
                => this with { Query = base.CreateSpecific(args) };
        }
        public static SqlAny<TEntity> ForAnyType<TEntity>(string sql)
            => Request<TEntity>.Any = new Request<TEntity>.SqlOfType(sql);
        public record SearchRequest : Sql
        {
            public static Sql Request { get; internal set; }
        }

        public record Request<TResult>
        {
            public static Sql Query { get; internal set; }
            public static SqlAny<TResult> Any { get; internal set; }
            public record SqlOfType(string Query = default) : SqlAny<TResult>(Query)
            {
                public SqlOfType GetSql<TInSql>(params string[] args) where TInSql : TResult
                    => this with { Query = CreateSpecific(args) };                
            }
            
    }
    }
}
