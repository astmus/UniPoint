using System.Collections.Specialized;
using System.Linq;
using MissBot.Abstractions;
using Newtonsoft.Json.Linq;
using static LinqToDB.Common.Configuration;

namespace MissCore.Collections
{
    public class DataMap : NameValueCollection, IDataMap
    {
        JToken first;
        public static DataMap Parse<TData>(TData value)
            => new DataMap().ParseTokens(JToken.FromObject(value));

        public void JoinData<TEntity>(TEntity entity)
             => first.Append(ParseTokens(JToken.FromObject(entity)).first);
        
        public TD Read<TD>(string name)
        {
            if (this[name] is string path && first.SelectToken(path) is JToken value)
                return value.ToObject<TD>();
            else return default;
        }

        private DataMap ParseTokens(JToken containerToken)
        {
            if (containerToken.Type == JTokenType.Object)
                foreach (var child in containerToken.Children<JProperty>())
                {
                    Add(child.Name, child.Path);
                    ParseTokens(child.Value);
                }
            else if (containerToken.Type == JTokenType.Array)
                foreach (var child in containerToken.Children())
                    ParseTokens(child);
            first = containerToken;
            return this;
        }
    }
}
