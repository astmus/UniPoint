using System.Runtime.CompilerServices;
using System.Text;
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
        protected MetaData MetaInformation
            => meta ?? (meta = new MetaData());
        
        protected T Set<T>(T value, [CallerMemberName] string name = default)
            => MetaInformation.Set(value, name);
        protected T Get<T>([CallerMemberName] string name = default)
            => MetaInformation.Get<T>(name);

        public virtual MetaData GetMetaData()
            => meta;
        
        

        public record MetaUnit(string Content = default, MetaData Data = default) : Unit
        {
            //string.Join(Environment.NewLine, unit.GetMetaData().Select(s => Convert.ToString(s.Value).Replace("]","").Replace(",", " = ")));
            protected override bool PrintMembers(StringBuilder builder)
            {
                var res = base.PrintMembers(builder);
                builder.Clear();
                builder.Append($"{Data?.ToString() ?? Convert.ToString(this)}");
                return res;
            }
        }
    }
}
