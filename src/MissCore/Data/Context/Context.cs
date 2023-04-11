using System.Collections.Concurrent;
using System.Transactions;
using MissBot.Abstractions;
using MissCore.Data.Identity;

namespace MissCore.Data.Context
{
    public class Context : ConcurrentDictionary<string, object>, IContext
    {  
        public T Get<T>(string name)
        {
            var result = default(T);
            if (TryGetValue(name, out var r) && r is T reference)
                result = reference;
            return result;
        }

        public T Get<T>()
        {
            var id = GetId<T>();
            var result = default(T);
            if (TryGetValue(id, out var r) && r is T reference)
                result = reference;

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
            => (T)AddOrUpdate(name ?? GetId<T>(),
                    (k, w) => w,
                    (k, o, w) => this[k] = w, value);

        protected virtual string GetId<T>()
            => Identifier<T>.TypeId;
    }

}





