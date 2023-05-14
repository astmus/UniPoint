using System.Collections.Specialized;
using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Common
{
    public class DataMap : NameValueCollection
    {
        JToken token;
        public DataMap(object data)
        {
            token = JToken.FromObject(data);
            ParseTokens(token);
        }
        public void Parse<TData>(TData value)
            => ParseTokens(JToken.FromObject(value));

        public TD Read<TD>(string name)
        {
            if (this[name] is string path && token.SelectToken(path) is JToken value)
                return value.ToObject<TD>();
            else return default;
        }


        private void ParseTokens(JToken containerToken)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (var child in containerToken.Children<JProperty>())
                {
                    Add(child.Name, child.Path);
                    ParseTokens(child.Value);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (var child in containerToken.Children())
                {
                    ParseTokens(child);
                }
            }
        }
    }
}
