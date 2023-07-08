

using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// <see cref="Poll"/> type
/// <remarks>
/// This enum is used only in the library APIs and is not present in types that are coming from
/// Telegram servers for compatibility reasons
/// </remarks>
/// </summary>
[JsonConverter(typeof(PollTypeConverter))]
public enum PollType
{
	/// <summary>
	/// Regular poll
	/// </summary>
	Regular = 1,

	/// <summary>
	/// Quiz
	/// </summary>
	Quiz
}


internal partial class PollTypeConverter : JsonConverter<PollType>
{
	public override void WriteJson(JsonWriter writer, PollType value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			PollType.Regular => "regular",
			PollType.Quiz => "quiz",
			(PollType)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override PollType ReadJson(
		JsonReader reader,
		Type objectType,
	PollType existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"regular" => PollType.Regular,
			"quiz" => PollType.Quiz,
			_ => 0,
		};
}
