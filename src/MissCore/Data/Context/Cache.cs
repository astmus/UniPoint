using System.Collections.Concurrent;
using MissBot.Abstractions.DataAccess;

namespace MissCore.Data.Context
{
    public class Cache : ConcurrentDictionary<string, object>
    {
        public T Get<T>(string identifier = default)
        {
            string id = identifier ?? Id<T>.Value;
            var result = default(T);
            if (TryGetValue(id, out var o) && o is T val) return val;

            return result;
        }
        public T Get<T>(Id<T> identifier)
            => Get<T>(identifier.id);
        public T Get<T>(Id identifier)
            => Get<T>(identifier.id);

        public TAny Any<TAny>()
        {
            return this.Where(x => x.Value is TAny).Select(s => s.Value).Cast<TAny>().FirstOrDefault();
        }
        public T Set<T>(T value, string identifier = default)
            => (T)AddOrUpdate(identifier ?? Id<T>.Value,
                    (k, w) => w,
                    (k, o, w) => this[k] = w, value);
        public T Set<T>(T value, Id identifier)
            => Set<T>(value, identifier.id);
    }
}





