using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// Type of the input media
/// </summary>
[JsonConverter(typeof(InputMediaTypeConverter))]
public enum InputMediaType
{
	/// <summary>
	/// Photo
	/// </summary>
	Photo = 1,

	/// <summary>
	/// Video
	/// </summary>
	Video,

	/// <summary>
	/// Animation
	/// </summary>
	Animation,

	/// <summary>
	/// Audio
	/// </summary>
	Audio,

	/// <summary>
	/// Document
	/// </summary>
	Document
}


internal partial class InputMediaTypeConverter : JsonConverter<InputMediaType>
{
	public override void WriteJson(JsonWriter writer, InputMediaType value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			InputMediaType.Photo => "photo",
			InputMediaType.Video => "video",
			InputMediaType.Animation => "animation",
			InputMediaType.Audio => "audio",
			InputMediaType.Document => "document",
			(InputMediaType)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override InputMediaType ReadJson(
		JsonReader reader,
		Type objectType,
	InputMediaType existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"photo" => InputMediaType.Photo,
			"video" => InputMediaType.Video,
			"animation" => InputMediaType.Animation,
			"audio" => InputMediaType.Audio,
			"document" => InputMediaType.Document,
			_ => 0,
		};
}
