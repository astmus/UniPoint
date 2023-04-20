using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace MissBot.Abstractions
{
    public record ValueUnit : Unit, IFormattable
    {
        //public static BotUnion Parse(JArray values)
        //    =>  new BotUnion(values.Children<JObject>().Select(s => Parse(s)));
        public bool HasMetadata()
            => meta != null;
        public virtual bool IsSimpleUnit()
            => true;
        public static MetaData Parse(object value)
        {
            var p = System.Text.Json.JsonSerializer.SerializeToDocument(value);

            MetaData parsed = new MetaData();
            
                if (p.RootElement.ValueKind is JsonValueKind.Array)
                {
                    var item = Activator.CreateInstance(value.GetType().GetGenericArguments()[0]);
                    p = System.Text.Json.JsonSerializer.SerializeToDocument(item);
                }

                foreach (var item in p.RootElement.EnumerateObject())
                    parsed.Set(item.Value.ToString(), item.Name);
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

        public virtual string ToString(string? format, IFormatProvider? formatProvider)
        {
            var s = Parse(this);
            return string.Join("     ", s.GetAll().Select(s => s.Item2))+"\n";
            
        }

        public record MetaUnit(string Content = default, MetaData Data = default) : Unit
        {
            //string.Join(Environment.NewLine, unit.GetMetaData().Select(s => Convert.ToString(s.Value).Replace("]","").Replace(",", " = ")));
            public override string ToString()
            {
                return $"{Data.ToString() ?? base.ToString()}";
            }
            protected override bool PrintMembers(StringBuilder builder)
            {
                var res = base.PrintMembers(builder);
                builder.Clear();
                builder.Append($"{nameof(Content)}: {Content}");
                return res;
            }
        }
    }
}
