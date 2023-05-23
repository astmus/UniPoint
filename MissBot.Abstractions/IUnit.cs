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
        IMetaData Meta { get; }
        public string Format(Formats format = Formats.Line | Formats.PropertyNames);
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
