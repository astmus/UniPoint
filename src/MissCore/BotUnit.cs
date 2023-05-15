using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore.Bot;
using MissCore.Collections;

namespace MissCore
{

    public static class BotUnit<TEntity> where TEntity : Unit
    {
        public class Collection : Unit<TEntity>.Collection { };
        public record Content : ContentUnit<TEntity>;
        public record Request<TData> where TData : Unit<TEntity>;

        public static TEntityUnit Instance<TEntityUnit>() where TEntityUnit : Unit<TEntity>
            => Unit<TEntityUnit>.Sample;
        public static IRepositoryCommand Query(Action<TEntity> selector)
        {
            var entity = Unit<TEntity>.Sample with { };
            selector(entity);
            //return new SQLCommand<TEntity>(Meta.ToWhere());
            return null;
        }

        //public static BotRequest Entities<TBotEntity>() where TBotEntity : class
        //    => new BotRequest($"{BotUnits} WHERE Entity = '{Unit<TBotEntity>.Key}'");
        //public static BotRequest Actions<TAction>() where TAction : BotActionRequest
        //    => new BotRequest($"{BotUnits} WHERE Entity = '{Unit<TAction>.Key}'", SQLType.JSONPath | SQLType.JSONNoWrap);
        //public static BotRequest Units<TBotUnit>()
        //    => new BotRequest($"{BotUnits} WHERE Entity = '{Unit<TBotUnit>.Key}'", SQLType.JSONPath | SQLType.JSONNoWrap);
        //public static BotRequest Commands<TCommand>() where TCommand : BotCommand
        //    => new BotRequest($"{BotUnits} WHERE Entity = '{Unit<BotCommand>.Key}' AND Command = '/{Unit<TCommand>.Key}'", SQLType.JSONPath | SQLType.JSONNoWrap);

        public const string Empty = "SELECT 1";
        public const string SelectFrom = "SELECT * FROM ##";
        public const string Select = "SELECT * FROM ##";
        public const string BotUnits = $"SELECT * FROM ##{nameof(BotUnit)}s ";

        public const string SelectFirst = $"SELECT TOP 1 * FROM ##{nameof(BotUnit)}s ";
        //public const string JSONNoWrap = ", WITHOUT_ARRAY_WRAPPER";

        //public static readonly string[] AllFileds = { "*" };
        //public static class Templates
        //{
        //    public const string SelectAllFrom = "SELECT * FROM ##{0}";
        //    public const string SelectFrom = "SELECT {1} FROM ##{0}";
        //    public const string JSONAuto = "{0} FOR JSON AUTO";
        //    public const string JSONPath = "{0} FOR JSON PATH";
        //    public const string JSONRoot = ", ROOT('{0}')";
        //    public const string Entity = "SELECT * FROM ##{0} [{0}] INNER JOIN ##BotUnits Commands ON Commands.Entity = [{0}].EntityName";
        //}
        public abstract record Response : BaseResponse<TEntity>;

    }
}
