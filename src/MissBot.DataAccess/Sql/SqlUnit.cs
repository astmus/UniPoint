using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;

namespace MissBot.DataAccess.Sql
{
    //public delegate IEnumerable<string> FieldNamesSelector<TUnit>(TUnit entity);
    
    public record SQL<TUnit>(string rawSql = default) : BotRequest where TUnit:class
    {
        public static readonly string EntityName = typeof(TUnit).Name;
        public  SQLCommand Command
            => rawSql != null ? rawSql : global::MissBot.Abstractions.DataAccess.Unit.Parse<TUnit>(global::MissBot.Abstractions.Unit<TUnit>.Sample);
    
        public record Query(FieldNamesSelector<TUnit> selector) : SQL<TUnit>
        {         
            public  SQLCommand Command
                => Abstractions.DataAccess.Unit.Entities<TUnit>(selector).Command;
        }
        public record Query<TResult>(FieldNamesSelector<TResult> selector = default) : SQL<TUnit> where TResult: TUnit
        {            
            public static readonly Query<TResult> Instance = new Query<TResult>();
            public  SQLCommand Command
                => Abstractions.DataAccess.Unit.Entity<TResult>(selector);
        }
        public record Request : SQL<TUnit>
        {
            public  SQLCommand Command
                => Abstractions.DataAccess.Unit.Entity<TUnit>();
        }
    }
}
