using System.Collections.Concurrent;
using MissBot.Abstractions.DataAccess;

namespace MissCore.Data.Context
{
    public class Cache : ConcurrentDictionary<string, object>
    {
        public T Get<T>(Id identifier = default)
        {
            string id = identifier?.id ?? Id<T>.Value;
            var result = default(T);
            if (TryGetValue(id, out var o) && o is T val) return val;

            return result;
        }
        public T Get<T>(Id<T> identifier)
        {
            string id = identifier?.id ?? Id<T>.Value;
            var result = default(T);
            if (TryGetValue(id, out var o) && o is T val) return val;

            return result;
        }
        public TAny Any<TAny>()
        {
            return this.Where(x => x.Value is TAny).Select(s => s.Value).Cast<TAny>().FirstOrDefault();
        }

        public T Set<T>(T value, Id identifier = default)
            => (T)AddOrUpdate(identifier?.id ?? Id<T>.Value,
                    (k, w) => w,
                    (k, o, w) => this[k] = w, value);
    }
}





