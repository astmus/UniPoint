
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissBot.Abstractions
{
    public interface IBotEntity
    {
        string Entity { get; }
    }
    public interface IBotDataEntity
    {
        string Payload { get; set; }
    }
    public interface IBotUnit : IBotEntity, IBotDataEntity
    {
        string Unit { get; set; }
        string Description { get; set; }
        string Template { get; set; }
        string this[int index]
        {
            get;
        }
    }
    
    public interface IBotUnit<TUnit> : IBotEntity
    {        
        IReplyMarkup GetUnitActions<TSub>(TSub unit) where TSub : Unit;
        public IEnumerable<IBotUnit> Units { get; }
    }
}
