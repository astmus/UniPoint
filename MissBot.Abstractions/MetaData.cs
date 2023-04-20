using System.Collections.Concurrent;
using System.Runtime.CompilerServices;

namespace MissBot.Abstractions
{
    public class MetaData : ConcurrentDictionary<string, object>
    {
        public T Get<T>([CallerMemberName] string name = default)
        {
            if (TryGetValue(name, out var r) && r is T value)
                return value;
            return default(T);
        }
        public override string ToString()
            =>
                Count > 0 ? string.Join('\n', this.OrderBy(ob=> ob.Key).Select(s => Convert.ToString(s.Key ?? s.Value))) : base.ToString() ;
        
        public TAny FirstOrNull<TAny>(string name)
            => this.Where(x => x.Key == name && x.Value is TAny).Select(s => s.Value).Cast<TAny>().FirstOrDefault();
        public object? AnyFirst(string name)
            => this.Where(x => x.Key == name).Select(s => s.Value).FirstOrDefault();
        public string GetAll()
            => string.Join(" ",this.Select(s => s.Key + " = " + s.Value));
        public T Set<T>(T value, [CallerMemberName] string name = default)
        {
            AddOrUpdate<T>(name,
                (k, w) => w,
                (k, o, w) => this[k] = w,
                value);
            return value;
        }
    }
}
