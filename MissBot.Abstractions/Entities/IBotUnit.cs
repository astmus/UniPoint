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
            => Parameters?.Split(";",StringSplitOptions.RemoveEmptyEntries).DefaultIfEmpty();       
    }

    public interface IBotUnit<TUnit> : IBotEntity
    {
        void SetUnitActions<TSub>(TSub unit) where TSub : BaseUnit;
        public IEnumerable<IBotUnit> Units { get; }
    }
}
