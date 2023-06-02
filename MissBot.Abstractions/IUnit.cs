using MissBot.Abstractions.Actions;

namespace MissBot.Abstractions
{
    public interface IUnit<in TUnit> : IUnit
    {
        IUnitItem GetItem(int index);        
        IMetaData Meta { get; }
    }
    public interface IUnit
    {
        object Identifier { get; }
        IActionsSet Actions { get; }     
    }
}
