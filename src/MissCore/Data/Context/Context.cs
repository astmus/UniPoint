using System.Collections.Concurrent;
using System.Transactions;
using MissBot.Abstractions;
using MissCore.Data.Identity;
using Newtonsoft.Json.Linq;

namespace MissCore.Data.Context
{
    public class Context : ConcurrentDictionary<string, object>, IContext
    {
        public DataMap Map { get; protected set; }
        public T Get<T>(string name)
        {
            var result = default(T);
            if (TryGetValue(name, out var r) && r is T reference)
                result = reference;
            return result;
        }

        public T Get<T>()
        {
            var id = Id.Of<T>();
            var result = default(T);
            if (TryGetValue(id, out var r) && r is JToken reference)
                result = reference.ToObject<T>();
            if (TryGetValue(id, out var o) && o is T val)
                result = val;

            return result;
        }

        public TAny Any<TAny>()
        {            
            return this.Where(x=> x.Value is TAny).Select(s=> s.Value).Cast<TAny>().FirstOrDefault();
        }

        public T Get<T>(Predicate<string> filter = null)
        {
            var result = default(T);
            if (filter == null)
                return Get<T>();
            var accept = Keys.Where(k => filter(k)).FirstOrDefault() ?? string.Empty;
            result = Get<T>(accept);
            return result is not null ? result : default;
        }

        public T Set<T>(T value, string name = null)
            => (T)AddOrUpdate(name ?? Id.Of<T>(),
                    (k, w) => w,
                    (k, o, w) => this[k] = w, value);


    }

}





