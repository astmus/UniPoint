using System.Collections.Concurrent;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace MissBot.Abstractions
{
    public class MetaData : NameValueCollection
    {
        ConcurrentDictionary<string, object> dictionary = new ConcurrentDictionary<string, object>();
        internal IEnumerable<KeyValuePair<string, object>> KeyPairs
                => dictionary;
        public string GetByName([CallerMemberName] string name = default)
        {
           return this[name];
        }
        public override string ToString()
            =>
                Count > 0 ? string.Join(':', this.AllKeys.Select(key => key)) : base.ToString();

        //public void Set<T>(string name, T value)
        //    =>  this[name] = Convert.ToString(value);

        public T Get<T>(string name)
        {
            var result = default(T);
            if (dictionary.TryGetValue(name, out var r) && r is T reference)
                result = reference;
            return result;
        }
        //public object Get(string name)
        //{
        //    if (dictionary.TryGetValue(name, out var r) && r is not null)
        //        return r;
        //    else return string.Empty;
        //}

        public T Set<T>(string name, T value)
            => (T)dictionary.AddOrUpdate(name,
                    (k, w) => w,
                    (k, o, w) => dictionary[k] = w, value);
    }
}
