namespace MissBot.Abstractions
{
    public interface IMetaData
    {
        public IMetaItem GetItem(int index);
    }

    public interface IMetaItem : IFormattable
    {
    }
}
