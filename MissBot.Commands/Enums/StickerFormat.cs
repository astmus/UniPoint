
using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// Format of the <see cref="Sticker"/>
/// </summary>
[JsonConverter(typeof(StickerFormatConverter))]
public enum StickerFormat
{
	/// <summary>
	/// Static <see cref="Sticker"/>
	/// </summary>
	Static = 1,
	/// <summary>
	/// Animated <see cref="Sticker"/>
	/// </summary>
	Animated,
	/// <summary>
	/// Video <see cref="Sticker"/>
	/// </summary>
	Video,
}


internal partial class StickerFormatConverter : JsonConverter<StickerFormat>
{
	public override void WriteJson(JsonWriter writer, StickerFormat value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			StickerFormat.Static => "static",
			StickerFormat.Animated => "animated",
			StickerFormat.Video => "video",
			(StickerFormat)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override StickerFormat ReadJson(
		JsonReader reader,
		Type objectType,
	StickerFormat existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"static" => StickerFormat.Static,
			"animated" => StickerFormat.Animated,
			"video" => StickerFormat.Video,
			_ => 0,
		};
}
