using System.Text;
using MissBot.Abstractions;
using MissBot.Abstractions.Presentation;
using MissCore.Data;
using MissCore.Data.Collections;
using MissCore.Presentation.Decorators;
using Newtonsoft.Json.Linq;

namespace MissCore.Presentation.Convert
{
    public class UnitSerializeConverter<TUnit> : JsonConverter<IUnit<TUnit>> where TUnit : class
    {
        protected internal ref struct TabWriter
        {
            private readonly ReadOnlySpan<char> _spaces;
            private readonly StringBuilder sb = new StringBuilder();
            private readonly UnitSerializeConverter<TUnit> _parent;
            public bool IsEmpty
                => sb.Length == 0;

            public TabWriter(UnitSerializeConverter<TUnit> parent)
            {
                _parent = parent;
                _spaces = string.Join(null, Enumerable.Repeat(" ", 32)).AsSpan();
            }

            public override string ToString()
                => sb.ToString();

            public void Append(JProperty source, int shift)
            {
                _parent.Decorator.SetComponent(new UnitItem(source));
                //Console.WriteLine($"{_parent.Decorator.Serialize()}\n{_spaces[..(shift * tab)]}");
                sb.Append($"{_parent.Decorator.Serialize()}\n{_spaces[..(shift * tab)]}");
            }
        }

        IUnitItemDecorator Decorator;
        public override void WriteJson(JsonWriter writer, IUnit<TUnit>? value, JsonSerializer serializer)
        {
            if (value == null) return;
            var tabWriter = new TabWriter(this);
            Decorator = new BoldNameUnitItemDecorator();
            MetaType t = MetaType.Item;
            switch (t)
            {
                case MetaType.Null:
                case MetaType.Empty:
                    break;
                case MetaType.Value when value is Unit<TUnit> data && data.UnitData is JValue val:
                    writer.WriteValue(val.Value);
                    break;
                case MetaType.Item when value is Unit<TUnit> data && data.UnitData is JProperty prop:
                    Serialize(ref tabWriter, prop);
                    break;
                case MetaType.Unit when value is Unit<TUnit> data && data.UnitData is JObject obj:
                    Serialize(ref tabWriter, obj);
                    break;
                case MetaType.Union when value is Unit<TUnit> data && data.UnitData is JArray array:
                    Serialize(ref tabWriter, array);
                    break;
                default:
                    Serialize(ref tabWriter, JToken.FromObject(value));
                    break;
            }

            if (!tabWriter.IsEmpty)
                writer.WriteValue(tabWriter.ToString());
        }

        protected const int tab = 3;
        protected const int ind = 1;

        protected virtual JToken GetItem(JToken token) => token switch
        {
            JProperty prop when prop.Value is JValue => prop,
            JProperty prop when prop.Value is JObject obj => obj,
            JProperty prop => prop,
            JValue val => val.Parent,
            JObject obj => obj.First,
            _ => null
        };

        protected virtual void Serialize(ref TabWriter tabWriter, JToken root, int shift = 0)
        {
            foreach (var item in root)
                switch (GetItem(item))
                {
                    case JObject targetObj:
                        foreach (var child in targetObj.Children<JProperty>())
                            tabWriter.Append(child, shift + ind);
                        break;
                    case JProperty prop when prop.Value is JObject obj:
                        foreach (var child in obj.Children<JObject>())
                            Serialize(ref tabWriter, child, shift + ind);
                        break;
                    case JValue value when value.Parent is JProperty prop:
                        Serialize(ref tabWriter, prop, shift + ind);
                        break;
                    case JProperty prop:
                        tabWriter.Append(prop, shift + ind);
                        break;
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
