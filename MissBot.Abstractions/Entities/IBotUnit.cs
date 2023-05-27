using MissBot.Abstractions.Actions;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissBot.Abstractions.Entities
{
    public interface IBotEntity
    {
        string Unit { get; set; }
        string Entity { get; }
    }
    public interface IBotDataEntity
    {
        string Payload { get; set; }
    }
    public interface IBotUnit : IBotEntity, IBotDataEntity
    {
        string Description { get; set; }
        string Template { get; set; }
        string Parameters { get; }
        string this[int index]
            => Parameters?.Split(";").ElementAtOrDefault(index);
        IEnumerable<string> GetParameters()
            => Parameters?.Split(";",StringSplitOptions.RemoveEmptyEntries);
        string Format(params object[] parameters)
            => string.Format(null, Payload, parameters);
    }

    public interface IBotUnit<TUnit> : IBotEntity
    {
        IActionsSet GetUnitActions<TSub>(TSub unit) where TSub : UnitBase;
        public IEnumerable<IBotUnit> Units { get; }
    }
}
