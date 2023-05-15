using System.Collections.Specialized;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Collections
{
    public class DataMap : NameValueCollection, IDataMap
    {
        JToken first;
        public DataMap(object data)
        {
            first = JToken.FromObject(data);
            ParseTokens(first);
        }
        public JToken Parse<TData>(TData value)
            => ParseTokens(JToken.FromObject(value));

        public TD Read<TD>(string name)
        {
            if (this[name] is string path && first.SelectToken(path) is JToken value)
                return value.ToObject<TD>();
            else return default;
        }

        private JToken ParseTokens(JToken containerToken)
        {
            if (containerToken.Type == JTokenType.Object)
                foreach (var child in containerToken.Children<JProperty>())
                {
                    Add(child.Name, child.Path);
                    ParseTokens(child.Value);
                }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (var child in containerToken.Children())
                    ParseTokens(child);
            }
            return first;
        }
    }
}
