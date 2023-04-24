using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.DataAccess
{
    public record SQLResult(uint AffectedRows, int ErrorCode = 0, string Description = default);
    public record SQLResult<TQuery>(ISQLQuery<TQuery> request, int ErrorCode = 0, string Description = default);
    public record SQLAction : BotAction<SQLUnit>;
    public record SQLUnit<TCommand>(SQLCommand Command) : SQLUnit where TCommand:class
    {
    }
        public record SQLUnit : Unit, ISQL
    {
        public SQLResult Result { get; set; }
        public virtual SQLCommand Command { get; }
    }


}
