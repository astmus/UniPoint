using System.Numerics;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Primitives;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using MissCore.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static LinqToDB.Common.Configuration;

namespace BotService.Internal
{
    //public class UnitConverter : JsonConverter<BaseUnit>
    //{
    //    public override BaseUnit? ReadJson(JsonReader reader, Type objectType, BaseUnit? existingValue, bool hasExistingValue, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    public override void WriteJson(JsonWriter writer, BaseUnit? value, JsonSerializer serializer)
    //    {
    //        throw new NotImplementedException();
    //    }

    //    UnitConverter<TUnit> GetConverter<TUnit>() where TUnit : BaseUnit
    //        => new UnitConverter<TUnit>();
    //}--
    //{
    public class KnownTypesBinder : ISerializationBinder
    {
        public IList<Type> KnownTypes
        { get; init; } = Assembly.GetCallingAssembly().GetTypes().Where(w => w.IsAssignableTo(typeof(BaseUnit))).ToList();

        public Type BindToType(string assemblyName, string typeName)
        {
            return KnownTypes.SingleOrDefault(t => t.Name == typeName);
        }

        public void BindToName(Type serializedType, out string assemblyName, out string typeName)
        {
            assemblyName = null;
            typeName = serializedType.Name;
        }
    }
    public class UnitConverter<TUnit> : JsonConverter where TUnit : BaseUnit,IUnit
    {
        static readonly string shiftString = string.Join(' ',Enumerable.Repeat<char>(' ', 128));
        readonly record struct ShiftStringBuilder(StringBuilder builder)
        {
            public ShiftStringBuilder Append(string add, ref byte shift)
            {
                builder.Append(shiftString.AsSpan(0, shift));
                builder.AppendLine(add);
                return this;
            }
            public override string ToString()
            => builder.ToString();
        }
        public override bool CanConvert(Type objectType)
        {
            Console.WriteLine(objectType.Name+ objectType.IsAssignableFrom(typeof(TUnit)));
            return objectType.IsAssignableTo(typeof(TUnit));
        }

        public override object? ReadJson(JsonReader reader, Type objectType, object? existingValue, JsonSerializer serializer)
        {
            if (reader.Value is string s)
                return Activator.CreateInstance<TUnit>();
            return default;
        }
        public override void WriteJson(JsonWriter writer, object? value, JsonSerializer serializer)
        {
            if (value is not TUnit unit) return;

            ShiftStringBuilder builder = new ShiftStringBuilder(new StringBuilder());
            MetaItemDecorator decor = new MetaItemDecorator();
            Serialize(ref builder, 0, ref decor, (unit.Meta as MetaData).root);
            writer.WriteValue(builder.ToString());
        }
        static ShiftStringBuilder Serialize(ref ShiftStringBuilder builder, byte shift, ref MetaItemDecorator decor, JToken data)
        {
            JToken GetItem(JToken token) => token switch
            {
                JProperty prop when prop.Value is JValue value => value,
                JProperty prop when prop.Value is JObject obj => obj,
                JObject obj => obj.First,
                _ => null
            };
            JToken? current = data;
            foreach (var item in data.Reverse())
            {
                IMetaItem mitem;
                JProperty property = null;
                switch (GetItem(item))
                {                    
                    case JObject obj when obj.Parent is JProperty prop:
                        property = prop;
                        shift += 4;
                        Serialize(ref builder, shift, ref decor, obj);             
                        break;
                    case JValue value when value.Parent is JProperty prop:
                        property = prop;
                        break;
                    case JProperty prop:
                        property = prop;
                        break;
                }
                        mitem = new MetaItem(property);
                        decor.SetComponent(ref mitem);
                        builder.Append(decor.Serialize(), ref shift);
            }
       
                return builder;
        }



    }
}
