namespace MissBot.Abstractions
{
    public interface IUnit<TUnit> : IUnit
    {
        string this[string path]
        {
            get;
        }
    }
    public interface IUnit
    {
        IMetaItem GetItem(int index);
        
        IMetaData Meta { get; }
        string Format(Formats? format);
        [Flags]
        public enum Formats : uint
        {
            UnitName = 1,
            Line = 2,
            Table = 4,
            PropertyNames = 8
        }
    }
}
