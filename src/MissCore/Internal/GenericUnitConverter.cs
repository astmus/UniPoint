using MissBot.Abstractions;
using Newtonsoft.Json.Linq;

namespace MissCore.Internal
{
	internal class GenericUnitConverter<TUnit> : JsonConverter<TUnit> where TUnit : class, IUnit
	{

		public override bool CanRead => false;
		public override bool CanWrite => true;

		public override TUnit? ReadJson(JsonReader reader, Type objectType, TUnit? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			var s = reader.ReadAsString();
			var r = JsonConvert.DeserializeObject<TUnit>(s);
			return r;
		}

		public override void WriteJson(JsonWriter writer, TUnit? value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			var iterator = value.UnitEntities;

			if (iterator.MoveNext())
				do
				{
					if (iterator.Current is JToken token)
						token.WriteTo(writer);
				}
				while (iterator.MoveNext());

			writer.WriteEnd();
		}
	}
}
