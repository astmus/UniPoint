using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
    public class MetaCollection
    {
        public MetaData Metadata { get; protected set; }
        public MetaCollection(IEnumerable<JToken> items)
        {
            Metadata = MetaData.Parse(items.FirstOrDefault());            
        }
    }

    public class MetaData<T> : MetaData where T : JToken
    {
        public MetaData(T data) : base(data)
        {
        }

        public override TD GetData<TD>()
        {
            if (this[typeof(TD).Name] is string path)
                return Pointer.SelectToken(path).ToObject<TD>();
            else return default;
        }
    }

    public class MetaData : ListDictionary
    {
        public JToken Pointer;
        public MetaData(JToken data)
        {
            Pointer = data; //JToken.FromObject(data);
            ParseTokens(Pointer);
        }
        public string Value
        =>            
            string.Join(" ", this.Values.Cast<string>().Select(s=> Pointer.SelectToken(s).ToString()));
       
        public static MetaData Parse<TData>(TData token)
        {
            return new MetaData(JToken.FromObject(token));
        }
        protected void ParseTokens(JToken containerToken)
        {
            if (containerToken.Type == JTokenType.Object)
            {
                foreach (JProperty child in containerToken.Children<JProperty>())
                {
                    this.Add(child.Name, child.Path);
                    ParseTokens(child.Value);
                }
            }
            else if (containerToken.Type == JTokenType.Array)
            {
                foreach (JToken child in containerToken.Children())
                {
                    ParseTokens(child);
                }
            }
        }

        public virtual TD GetData<TD>()
        => default;
        public object GetByName([CallerMemberName] string name = default)
        {
            return this[name];
        }

        //public void Set<T>(string name, T value)
        //    =>  this[name] = Convert.ToString(value);

        public T Get<T>(string name)
            => default;
        //{
        //    var result = default(T);
        //    if (dictionary.TryGetValue(name, out var r) && r is T reference)
        //        result = reference;
        //    return result;
        //}
        //public object Get(string name)
        //{
        //    if (dictionary.TryGetValue(name, out var r) && r is not null)
        //        return r;
        //    else return string.Empty;
        //}

        public T Set<T>(string name, T value)
            => value;
        //    => (T)dictionary.AddOrUpdate(name,
        //            (k, w) => w,
        //            (k, o, w) => dictionary[k] = w, value);
    }

}
