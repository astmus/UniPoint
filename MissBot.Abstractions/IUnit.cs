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
        IActionsSet Actions { get; }

        [Flags]
        public enum DisplayFormat : byte
        {            
            Line = 1,
            Table = 2,
            PropertyNames = 4
        }
    }
}
