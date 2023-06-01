using System.Collections.Specialized;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Data.Collections
{
    public class DataMap : NameValueCollection, IDataMap
    {
        JObject container;
        public DataMap() : base(StringComparer.OrdinalIgnoreCase)
        {
        }

        public static DataMap Parse<TData>(TData value)
            => new DataMap().Parse(JObject.FromObject(value));

        public void JoinData<TEntity>(TEntity entity)
             => Parse(JObject.FromObject(entity));

        public bool Contains(string name)
            => this[name] is string path && container.SelectToken(path) is JToken value;
        public TEntity ReadObject<TEntity>(string name)
        {
            if (this[name] is string path && container.SelectToken(path) is JToken value)
                return value.ToObject<TEntity>();
            return default;
        }

        private DataMap Parse(JObject containerToken)
        {
            if (container is JObject head)
                head.Merge(containerToken);
            else
                container = containerToken;
            return ParseTokens(containerToken);
        }

        private DataMap ParseTokens(JToken containerToken)
        {
            if (containerToken.Type == JTokenType.Object)
                foreach (var child in containerToken.Children<JProperty>())
                {
                    if (child.Name.IndexOf('_') > -1 /*&& child.Name.AsSpan() is ReadOnlySpan<char> span*/)
                        Add(child.Name.Replace("_", ""), child.Path);
                    else
                        Add(child.Name, child.Path);
                    ParseTokens(child.Value);
                }
            else if (containerToken.Type == JTokenType.Array)
                foreach (var child in containerToken.Children())
                    ParseTokens(child);
            return this;
        }
    }
}
