using System.Collections.Concurrent;
using MissCore.Abstractions;
using MissCore.Data.Identity;

namespace MissCore.Data.Context
{
    public class Context : ConcurrentDictionary<string, object>, IContext
    {
        public IServiceProvider Services { get; internal set; }
        public Context()
        {

        }
        public Context(IServiceProvider services)
            => Services = services;
        public T Get<T>(string name)
        {
            var result = default(T);
            if (TryGetValue(name, out var r) && r is T reference)
                result = reference;
            return result;
        }

        public T Get<T>()
        {
            var id = Identifier<T>.TypeId;
            var result = default(T);
            if (TryGetValue(id, out var r) && r is T reference)
                result = reference;

            return result;
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
            => (T)AddOrUpdate(name ?? Identifier<T>.TypeId,
                    (k, w) => w,
                    (k, o, w) => this[k] = w, value);
    }

}





