using System.Text;
using MissBot.Abstractions;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Presentation;
using MissCore.Data.Collections;
using MissCore.Presentation.Decorators;
using Newtonsoft.Json.Linq;

namespace MissCore.Presentation.Convert
{

    public class UnitConverter<TUnit> : JsonConverter<IUnit<TUnit>> where TUnit : BaseUnit
    {
        ref struct ShiftedWriter
        {
            private readonly ReadOnlySpan<char> _spaces;
            private readonly StringBuilder sb;

            public ShiftedWriter(StringBuilder builder)
            {

                _spaces = string.Join(null, Enumerable.Repeat(" ", 128)).AsSpan();
                sb = builder;
            }
            public override string ToString()
                => sb.ToString();
            public void Append(IUnitItemDecorator decorator, int shift, JProperty source)
            {
                decorator.SetComponent(new UnitItem(source));
                Console.WriteLine($"{decorator.Serialize()}\n{_spaces[..(shift * tab)]}");
                sb.Append($"{decorator.Serialize()}\n{_spaces[..(shift * tab)]}");
                //sb. = $"{decorator.Serialize()}\n{_spaces[..(shift + 1)]}";
                //_writer.WriteValue();
            }
        }

        public override void WriteJson(JsonWriter writer, IUnit<TUnit>? value, JsonSerializer serializer)
        {
            var shiftWriter = new ShiftedWriter(new StringBuilder());
            var decor = new BoldNameUnitItemDecorator();

            Serialize(shiftWriter, 0, decor, (value.Meta as MetaData).root);
            writer.WriteValue(shiftWriter.ToString());
        }

        const int tab = 3;
        const int ind = 1;
        static void Serialize(ShiftedWriter _shiftWriter, int shift, UnitItemSerializeDecorator decor, JToken data)
        {
            JToken GetItem(JToken token) => token switch
            {
                JProperty prop when prop.Value is JValue => prop,
                JProperty prop when prop.Value is JObject obj => obj,
                JProperty prop => prop,
                JValue val => val.Parent,
                JObject obj => obj.First,
                _ => null
            };
            //switch (data.Type)
            //{
            //    case JTokenType.Object:
            //        Serialize(_shiftWriter, (byte)(shift + inc), decor, GetItem(data));
            //        foreach (var child in data.Children<JProperty>())
            //            Serialize(_shiftWriter, (byte)(shift + inc), decor, child);
            //        break;
            //    case JTokenType.Property:
                    
            //        break;
            //    case JTokenType.Array:
            //        foreach (var children in data.Children())
            //            foreach (var child in children.Children<JProperty>())
            //                Serialize(_shiftWriter, (byte)(shift + inc), decor, child);
            //        break;
            //}
            foreach (var item in data)
            {
                switch (GetItem(item))
                {
                    case JObject target when target.Parent is JProperty prop:
                        _shiftWriter.Append(decor, shift + ind, prop);                        
                        foreach (var child in prop.Children<JObject>())                      
                                Serialize(_shiftWriter, shift + ind, decor, child);
                        //Serialize(_shiftWriter, (byte)(shift + inc), decor, target);
                        break;
                    case JValue value when value.Parent is JProperty prop:
                        Serialize(_shiftWriter, shift + ind, decor, prop);
                        break;
                    case JProperty prop:
                        _shiftWriter.Append(decor, shift + ind, prop);
                        break;
                }
            }
        }
        public override IUnit<TUnit>? ReadJson(JsonReader reader, Type objectType, IUnit<TUnit>? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
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
    //public class KnownTypesBinder : ISerializationBinder
    //{
    //    public IList<Type> KnownTypes
    //    { get; init; } = Assembly.GetCallingAssembly().GetTypes().Where(w => w.IsAssignableTo(typeof(BaseUnit))).ToList();

    //    public Type BindToType(string assemblyName, string typeName)
    //    {
    //        return KnownTypes.SingleOrDefault(t => t.Name == typeName);
    //    }

    //    public void BindToName(Type serializedType, out string assemblyName, out string typeName)
    //    {
    //        assemblyName = null;
    //        typeName = serializedType.Name;
    //    }
    //}
}
