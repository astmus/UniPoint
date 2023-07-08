using Microsoft.Extensions.DependencyInjection;

using MissBot.Entities.Abstractions;

using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.Converters
{
	public class UnitActionsConverter : JsonConverter<IEnumerable<IEnumerable<IUnitAction>>> //where TAction : IEnumerable<IEnumerable<IUnitAction<TAction>>>
	{
		private readonly IServiceProvider sp;
		//static UnitActionsConverter()
		//    => JsonConvert.DefaultSettings = () =>
		//    {
		//        return new JsonSerializerSettings()
		//        {
		//            NullValueHandling = NullValueHandling.Ignore,
		//            DefaultValueHandling = DefaultValueHandling.Ignore,
		//            MissingMemberHandling = MissingMemberHandling.Ignore
		//        };
		//    };

		string _chapter;
		public UnitActionsConverter(string chapter)
			=> _chapter = chapter;

		public UnitActionsConverter(IServiceProvider sp)
		{
			this.sp = sp;
		}
		public override bool CanWrite => true;
		public override bool CanRead => false;

		public override IEnumerable<IEnumerable<IUnitAction>> ReadJson(JsonReader reader, Type objectType, IEnumerable<IEnumerable<IUnitAction>> existingValue, bool hasExistingValue, JsonSerializer serializer)
		{
			throw new NotImplementedException();
		}

		public override void WriteJson(JsonWriter writer, IEnumerable<IEnumerable<IUnitAction>> value, JsonSerializer serializer)
		{
			writer.WriteStartObject();
			writer.WritePropertyName(_chapter);
			writer.WriteStartArray();
			writer.WriteStartArray();

			foreach (var column in value)
				foreach (var item in column)
				{
					var obj = JObject.FromObject(item);
					obj.WriteTo(writer);
				}

			writer.WriteEndArray();
			writer.WriteEndArray();
			writer.WriteEndObject();
			// throw new NotImplementedException();
		}
	}

	internal class UnitActionConverter<TAction> : CustomCreationConverter<TAction> where TAction : class, IUnitAction
	{
		private readonly IServiceProvider sp;
		public UnitActionConverter()
		{
			var i = 0;
		}
		public UnitActionConverter(IServiceProvider sp)
		{
			this.sp = sp;
		}
		public override TAction Create(Type objectType)
		{
			return ActivatorUtilities.GetServiceOrCreateInstance<TAction>(sp);

		}
	}
}
