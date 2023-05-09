using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using MissBot.Abstractions.DataAccess;

namespace MissBot.Abstractions
{
    [JsonObject]
    public record ValueUnit : Unit
    {

        public bool HasMetadata()
            => meta != null;

        static readonly ListDictionary Cache = new ListDictionary();
        
        public static MetaData Get<TUnit>(TUnit value)
        {
            var key = Unit<TUnit>.UnitName;            
            if (!Cache.Contains(key))
            {
                var root = System.Text.Json.JsonSerializer.SerializeToDocument(value);

                MetaData parsed = new MetaData();

                if (root.RootElement.ValueKind is JsonValueKind.Array)
                {
                    var type = value.GetType();
                    var item = Activator.CreateInstance(type.GetGenericArguments()?.FirstOrDefault() ?? type);
                    root = System.Text.Json.JsonSerializer.SerializeToDocument(item);
                }

                    Enumerate(root.RootElement);

                    void Enumerate(JsonElement element)
                    {
                        if (element.ValueKind == JsonValueKind.Object)
                        {
                            foreach (var item in element.EnumerateObject())
                            {
                            parsed[item.Name] = item.Value.GetRawText();
                            if (item.Value.ValueKind == JsonValueKind.Object)
                                {
                                    Enumerate(item.Value);
                                }
                                else
                                if (item.Value.ValueKind == JsonValueKind.Number)
                                {
                                //    parsed.Set(item.Name, item.Value.GetRawText());
                                parsed[item.Name]=item.Value.GetRawText();
                                }
                            }
                        }
                    }
                

                //foreach (var item in root.RootElement.EnumerateObject())
                //    parsed.Set(item.Name, item.Value.GetRawText());
                Cache.Add(key, parsed);
                return parsed;
            }
            return Cache[key] as MetaData;

        }


        MetaData meta;
        protected MetaData Meta
            => meta ?? (meta = new MetaData());

        public void Set<T>(T value, [CallerMemberName] string name = default)
            => Meta.Set(name, Meta.Set<T>(name, value));
        public T Get<T>([CallerMemberName] string name = default)
            => Meta.Get<T>(name) ?? (T)this[name];

        public virtual MetaData GetMetaData()
            => meta;
        public object this[string index]
        {
            get => Meta.Get(index);
        }

        //public string Serialize()
        //    => string.Join('\n', Data.Select(s => $"{s.Key} = {s.Value}"));

       
    }
}
