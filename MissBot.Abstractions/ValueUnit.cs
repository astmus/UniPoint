using System.Runtime.CompilerServices;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
    public record ValueUnit : Unit
    {
        public static BotUnion Parse(JArray values)
            =>  new BotUnion(values.Children<JObject>().Select(s => Parse(s)));
        public bool HasMetadata()
            => meta != null;
        public virtual bool IsSimpleUnit()
            => true;
        public static ValueUnit Parse(JObject value)
        {
            ValueUnit parsed = new ValueUnit();
            foreach (var item in value)
            {
                var replaced = Convert.ToString($"{item.Key}: {item.Value}");
                parsed.Set(replaced, item.Key);
            }
            return parsed;
        }
        MetaData meta;
        protected MetaData Meta
            => meta ?? (meta = new MetaData());

        protected T Set<T>(T value, [CallerMemberName] string name = default)
            => Meta.Set(value, name);
        protected T Get<T>([CallerMemberName] string name = default)
            => Meta.Get<T>(name);

        public virtual MetaData GetMetaData()
        {
            Meta.Set(Meta["Entity"], "Content");
            Meta["Entity"] = null;
            
            return Meta;
        }

    }
}
