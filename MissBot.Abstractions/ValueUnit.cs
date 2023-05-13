using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using MissBot.Abstractions.DataAccess;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions
{
    [JsonObject]
    public record ValueUnit : Unit
    {

        public bool HasMetadata()
            =>false;

        static readonly NameValueCollection Cache = new NameValueCollection();

        public static DataMap Get<TUnit>(TUnit value)
        {
            //var key = Unit<TUnit>.UnitName;
            return Unit<TUnit>.Parse(value);
        }
            


        


        MetaData meta;
        protected MetaData Meta
            => meta ?? (meta = new MetaData(JToken.FromObject(this)));

        public void Set<T>(T value, [CallerMemberName] string name = default)
            => Meta.Set(name, Meta.Set<T>(name, value));
        public T Get<T>([CallerMemberName] string name = default)
            => Meta.Get<T>(name);

        public virtual MetaData GetMetaData()
            => meta;
    

        //public string Serialize()
        //    => string.Join('\n', Data.Select(s => $"{s.Key} = {s.Value}"));

       
    }
}
