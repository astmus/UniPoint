using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Utils;
using MissCore.Collections;
using Newtonsoft.Json.Linq;

namespace MissCore.Data.Context
{
    public class Context : ConcurrentDictionary<string, object>, IContext
    {
        public DataMap Map { get; protected set; }
        public Context() : base(StringComparer.OrdinalIgnoreCase)
        {                
        }

        public T Get<T>()
        {
            string id = Id<T>.Value;
            var result = default(T);
            if (TryGetValue(id, out var o) && o is T val)
                return val;

            result = Map.ReadObject<T>(id);
            return result;
        }    

        public T Take<T>([CallerMemberName] string name = default)
        {
            var id = name.ToSnakeCase();
            var result = default(T);
            if (TryGetValue(id, out var r) && r is T reference)
                return reference;

            result = Map.ReadObject<T>(id);
            return Set(result);
        }

        public TAny Any<TAny>()
            =>  this.Where(x => x.Value is TAny).Select(s => s.Value).Cast<TAny>().FirstOrDefault();

        public T Set<T>(T value, string name = null)
            => (T)AddOrUpdate(name ?? Id<T>.Value,
                    (k, w) => w,
                    (k, o, w) => this[k] = w, value);

        public T Take<T>(Id<T> identifier)
            => Take<T>(identifier.id);
    }

}





