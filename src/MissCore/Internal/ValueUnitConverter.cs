using System;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters;
using MissBot.Entities.Abstractions;
using MissCore.Data;
using MissCore.Data.Collections;
using Newtonsoft.Json.Linq;

namespace MissCore.Internal
{
	public readonly struct Percent : IProgress<Percent>
	{
		public void Report(Percent value)
		{
			throw new NotImplementedException();
		}
	}

	internal class GenericUnitConverter : JsonConverter<IGenericUnit>
	{
		//private readonly IServiceProvider sp;

		//public GenericUnitConverter(IServiceProvider sp)
		//	=> this.sp = sp;


		public override bool CanRead => false;
		public override bool CanWrite => true;

		public override IGenericUnit? ReadJson(JsonReader reader, Type objectType, IGenericUnit? existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			var s = reader.ReadAsString();
			//Task.Factory.
			var r = JsonConvert.DeserializeObject<IGenericUnit>(s);
			return r;
		}

		public override void WriteJson(JsonWriter writer, IGenericUnit? value, JsonSerializer serializer)
		{
			//writer.WriteStartObject();
			//var local = PropertyFacade.Instance with {  };
			var enumerat = value.GetEnumerator();

			while (enumerat.MoveNext())
			{
				writer.WritePropertyName(enumerat.Current.ToString());
				writer.WriteValue(enumerat.Current);
			}
			//var iterator = value.UnitEntities;

			//if (iterator.MoveNext())
			//	do
			//	{
			//		if (iterator.Current is JToken token)
			//			token.WriteTo(writer);
			//	}
			//	while (iterator.MoveNext());

			//writer.WriteEnd();
		}
	}

	internal class ValueUnitConverter : JsonConverter<ValueUnit>
	{
		System.Double d = 0.0;
		public override bool CanRead => false;
		public override bool CanWrite => true;
		public override ValueUnit ReadJson(JsonReader reader, Type objectType, ValueUnit existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, ValueUnit value, JsonSerializer serializer)
		{
			//	writer.WriteStartObject();
			var root = JToken.FromObject(value.Value);
			var iterator = root.Children().GetEnumerator();
			root.WriteTo(writer);

			if (iterator.MoveNext())
				do
					if (iterator.Current is JToken token)
						token.WriteTo(writer);
				while (iterator.MoveNext());

			//	writer.WriteEnd();
		}
	}
}
