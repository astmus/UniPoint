using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.Converters
{
    internal class GenericUnitConverter<TUnit> : JsonConverter where TUnit : IUnit
    {
        private readonly IServiceProvider sp;
        public GenericUnitConverter()
        {
            var i = 0;
        }
        public GenericUnitConverter(IServiceProvider sp)
        {
            this.sp = sp;
        }

        public override bool CanRead => false;
        public override bool CanConvert(Type objectType)
            => false;
        //{
        //    return objectType == typeof(TUnit);
        //}

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var s = reader.ReadAsString();
            var r = JsonConvert.DeserializeObject(s);
            return r;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var t = (TUnit)value;

            //if (t.Type != JTokenType.Object)
            //{
            //    t.WriteTo(writer);
            //}
            //else
            //{
            var o = JObject.FromObject(t);
            IList<string> propertyNames = o.Properties().Select(p => p.Name).ToList();

            o.AddFirst(new JProperty("Keys", new JArray(propertyNames)));

            o.WriteTo(writer);
            //  }

            //writer.WriteToken(JsonToken.StartObject, value);

            // writer.WriteToken(JsonToken.EndObject, value);
        }
    }
}
