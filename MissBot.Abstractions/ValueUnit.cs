using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;


namespace MissBot.Abstractions
{
    public record ValueUnit : Unit
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
            try
            {
                if (p.RootElement.ValueKind is JsonValueKind.Array)
                    foreach (var item in p.RootElement.EnumerateArray())
                        foreach (var item2 in item.EnumerateObject())
                            parsed.Set(item2.Value.ToString(), item2.Name);
                else
                    foreach (var item in p.RootElement.EnumerateObject())
                        parsed.Set(item.Value.ToString(), item.Name);
                return parsed;
            }catch(Exception e)
            {
                int i = 0;
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
                builder.Append($"{Data?.ToString() ?? nameof(Content)}");
                return res;
            }
        }
    }
}
