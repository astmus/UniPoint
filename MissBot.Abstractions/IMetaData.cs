using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
    public interface IMetaData
    {
        IUnitItem GetItem(int index);
        IUnitItem GetItem(string key);
        void SetItem<TItem>(string name, TItem item);
        object GetValue(string path);
        IEnumerable<string> Values { get; }
        IEnumerable<string> Keys { get; }
        void SetContainer<TContainer>(TContainer container) where TContainer : JToken;
        string StringValue { get; }
        int Count { get; }
        IEnumerable<IUnitItem> Items { get; }

    }


    public interface ISerializable
    {
        string Serialize();        
    }
    public interface IUnitItem<TName, TValue> : ISerializable
    {
        TName ItemName { get; }
        TValue ItemValue { get; }
    }
    public interface IUnitItem : IUnitItem<string, object>
    {

    }

}
