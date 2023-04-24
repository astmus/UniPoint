using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;

namespace MissBot.DataAccess.Sql
{
    //public delegate IEnumerable<string> FieldNamesSelector<TUnit>(TUnit entity);
    public record SQL<TUnit>(string rawSql = default) : SQLUnit where TUnit:class
    {
        public TUnit Entity { get; init; } = Unit<TUnit>.Sample;
        public static readonly string EntityName = typeof(TUnit).Name;
        public SQLUnit<TUnit> Unit { get;  }
        public override SQLCommand Command
            => rawSql != null ? rawSql : SQL.Parse<TUnit>(Entity);
        SQLCommand request;

        public record Query(FieldNamesSelector<TUnit> selector) : SQL<TUnit>
        {         
            public override SQLCommand Command
                => SQL.Entities<TUnit>(selector).Command;
        }
        public record Query<TResult>(FieldNamesSelector<TResult> selector = default) : SQL<TUnit> where TResult: TUnit
        {            
            public static readonly Query<TResult> Instance = new Query<TResult>();
            public override SQLCommand Command
                => SQL.Entity<TResult>(selector);
        }
        public record Request : SQL<TUnit>
        {
            public override SQLCommand Command
                => SQL.Entity<TUnit>();
        }
    }
}
