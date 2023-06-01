using System.Numerics;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.Primitives;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Presentation;
using MissBot.Abstractions.Utils;
using MissCore.Data.Collections;
using MissCore.Presentation.Decorators;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using static LinqToDB.Common.Configuration;

namespace MissCore.Presentation.Convert
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
    public class UnitConverter<TUnit> : JsonConverter<IUnit<TUnit>> where TUnit : BaseUnit
    {
        static readonly string shiftString = string.Join(' ', Enumerable.Repeat(' ', 128));
        record ShiftStringBuilder(StringBuilder builder)
        {
            public ShiftStringBuilder Append(string add, ushort shift)
            {
                builder.Append(shiftString.AsSpan(0, shift));
                builder.AppendLine(add);
                return this;
            }
            public override string ToString()
            => builder.ToString();
        }

        public override void WriteJson(JsonWriter writer, IUnit<TUnit>? value, JsonSerializer serializer)
        {
            var builder = new ShiftStringBuilder(new StringBuilder());
            var decor = new BoldNameUnitItemDecorator();
            Serialize(builder, 0, decor, (value.Meta as MetaData).root);
            writer.WriteValue(builder.ToString());
        }

        static ShiftStringBuilder Serialize(ShiftStringBuilder builder, ushort shift, UnitItemSerializeDecorator decor, JToken data)
        {
            JToken GetItem(JToken token) => token switch
            {
                JProperty prop when prop.Value is JValue value => value,
                JProperty prop when prop.Value is JObject obj => obj,
                JObject obj => obj.First,
                _ => null
            };

            void Add(JProperty prop)
            {
                var mitem = new UnitItem(prop);
                decor.SetComponent(mitem);
                builder.Append(decor.Serialize(), (ushort)(shift * 4));
            }

            var current = data;
            foreach (var item in data)
            {
                switch (GetItem(item))
                {
                    case JObject obj when obj.Parent is JProperty prop:
                        Add(prop);
                        Serialize(builder, ++shift, decor, obj);
                        break;
                    case JValue value when value.Parent is JProperty prop:
                        Add(prop);
                        break;
                    case JProperty prop:
                        Add(prop);
                        break;
                }
            }

            return builder;
        }
        public override IUnit<TUnit>? ReadJson(JsonReader reader, Type objectType, IUnit<TUnit>? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
