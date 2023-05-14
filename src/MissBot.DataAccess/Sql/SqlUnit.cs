using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;

namespace MissBot.DataAccess.Sql
{
    //public delegate IEnumerable<string> FieldNamesSelector<TUnit>(TUnit entity);

    public record SQL<TUnit>(string rawSql = default) : BotRequest where TUnit : class
    {
        public static readonly string EntityName = typeof(TUnit).Name;
        public SQLCommand Command
            => rawSql != null ? rawSql : global::MissBot.Abstractions.DataAccess.SqlUnit.Parse<TUnit>(global::MissBot.Entities.Common.Unit<TUnit>.Sample);

        public record Query(FieldNamesSelector<TUnit> selector) : SQL<TUnit>
        {
            public SQLCommand Command
                => Abstractions.DataAccess.SqlUnit.Entities<TUnit>(selector).Request;
        }
        public record Query<TResult>(FieldNamesSelector<TResult> selector = default) : SQL<TUnit> where TResult : TUnit
        {
            public static readonly Query<TResult> Instance = new Query<TResult>();
            public SQLCommand Command
                => Abstractions.DataAccess.SqlUnit.Entity<TResult>(selector);
        }
        public record Request : SQL<TUnit>
        {
            public SQLCommand Command
                => Abstractions.DataAccess.SqlUnit.Entity<TUnit>();
        }
    }
}
