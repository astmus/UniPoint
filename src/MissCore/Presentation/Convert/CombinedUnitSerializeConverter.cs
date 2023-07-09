using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Presentation.Convert
{

	public class CombinedUnitSerializeConverter<TUnit> : UnitSerializeConverter<TUnit> where TUnit : class
	{
		protected override JToken GetItem(JToken token) => token switch
		{
			JProperty prop when prop.Value is JArray arr => arr,
			_ => base.GetItem(token)
		};

		protected override void Serialize(ref TabWriter tabWriter, JToken root, int shift = 0)
		{
			foreach (JToken item in root)
			{
				switch (item)
				{
					case JArray arr:
						foreach (var child in arr.Children())
							Serialize(ref tabWriter, child, shift);
						break;
					case JObject target when target.Parent is JArray arr:
						//foreach (var child in arr)
						//foreach (var child in target.Children<JProperty>())
						Serialize(ref tabWriter, target, shift + ind);
						break;
					case JObject obj when obj.Parent is JProperty prop:
						tabWriter.Append(propertyFacede.Wrap(prop), shift + ind);
						//foreach (var child in prop.Children<JObject>())
						Serialize(ref tabWriter, obj, shift + ind);
						break;
					case JValue value when value.Parent is JProperty prop:
						tabWriter.Append(propertyFacede.Wrap(prop), shift + ind);
						break;
					case JProperty prop:
						tabWriter.Append(propertyFacede.Wrap(prop), shift + ind);
						break;
				}
			}
		}
		//public override void WriteJson(JsonWriter writer, IUnit<TUnit>? value, JsonSerializer serializer)
		//{
		//    if (value == null) return;
		//    var tabWriter = new TabWriter(new StringBuilder());
		//    var decor = new BoldNameUnitItemDecorator();

		//    switch (value.Meta)
		//    {
		//        case MetaData meta:
		//            if (meta.root is JValue val)
		//                writer.WriteValue(val.Value);
		//            else
		//                Serialize(tabWriter, 0, decor, meta.root);
		//            break;
		//        default:
		//            Serialize(tabWriter, 0, decor, JToken.FromObject(value));
		//            break;
		//    }

		//    if (!tabWriter.IsEmpty)
		//        writer.WriteValue(tabWriter.ToString());
		//}

		//static void Serialize(TabWriter _shiftWriter, int shift, UnitItemSerializeDecorator decor, JToken data)
		//{
		//    JToken GetItem(JToken token) => token switch
		//    {
		//        JProperty prop when prop.Value is JValue => prop,
		//        JProperty prop when prop.Value is JObject obj => obj,
		//        JProperty prop => prop,
		//        JValue val => val.Parent,
		//        JObject obj => obj.First,
		//        _ => null
		//    };

		//    foreach (var item in data)
		//    {
		//        switch (GetItem(item))
		//        {
		//            case JObject target when target.Parent is JProperty prop:
		//                _shiftWriter.Append(decor, shift + ind, prop);
		//                foreach (var child in prop.Children<JObject>())
		//                    Serialize(_shiftWriter, shift + ind, decor, child);
		//                //Serialize(_shiftWriter, (byte)(shift + inc), decor, target);
		//                break;
		//            case JValue value when value.Parent is JProperty prop:
		//                Serialize(_shiftWriter, shift + ind, decor, prop);
		//                break;
		//            case JProperty prop:
		//                _shiftWriter.Append(decor, shift + ind, prop);
		//                break;
		//        }
		//    }
		//}
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
