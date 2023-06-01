using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
    public interface IMetaData
    {
        IMetaItem GetItem(int index);
        IMetaItem GetItem(string key);
        void SetItem<TItem>(string name, TItem item);
        object GetValue(string path);
        IEnumerable<string> Values { get; }
        IEnumerable<string> Keys { get; }
        void SetContainer<TContainer>(TContainer container) where TContainer : JToken;
        string StringValue { get; }
        int Count { get; }
        IEnumerable<IMetaItem> Items { get; }

    }
    public interface IMetaItem<TName, TValue>
    {
        TName UnitName { get; }
        TValue UnitValue { get; }
        string Serialize();
    }
    public interface IMetaItem : IMetaItem<string, object>
    {

    }

}
