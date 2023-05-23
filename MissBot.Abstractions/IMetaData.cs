using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
    public interface IMetaData
    {
        IMetaItem GetItem(int index);
        void SetItem<TItem>(string name, TItem item);
        object GetValue(string path);
        IEnumerable<string> Values { get; }
        IEnumerable<string> Keys { get; }
        void SetContainer<TContainer>(TContainer container) where TContainer : JToken;
        string StringValue { get; }
            
    }

    public interface IMetaItem : IFormattable
    {
        string Format(Formats format);

        [Flags]
        public enum Formats : uint
        {
            NewLine = 1,
            B = 2,
            I = 4,
            Code = 8,
            Strike = 16,
            Under = 32,
            Pre = 64,
            Link = 128,
            BSection = 256,
            Percent = 512,
            Equal = 1024,
            Section = 2048
        }
    }
}
