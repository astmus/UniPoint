using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// The part of the face relative to which the mask should be placed.
/// </summary>
[JsonConverter(typeof(MaskPositionPointConverter))]
public enum MaskPositionPoint
{
	/// <summary>
	/// The forehead
	/// </summary>
	Forehead = 1,

	/// <summary>
	/// The eyes
	/// </summary>
	Eyes,

	/// <summary>
	/// The mouth
	/// </summary>
	Mouth,

	/// <summary>
	/// The chin
	/// </summary>
	Chin
}


internal partial class MaskPositionPointConverter : JsonConverter<MaskPositionPoint>
{
	public override void WriteJson(JsonWriter writer, MaskPositionPoint value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			MaskPositionPoint.Forehead => "forehead",
			MaskPositionPoint.Eyes => "eyes",
			MaskPositionPoint.Mouth => "mouth",
			MaskPositionPoint.Chin => "chin",
			(MaskPositionPoint)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override MaskPositionPoint ReadJson(
		JsonReader reader,
		Type objectType,
	MaskPositionPoint existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"forehead" => MaskPositionPoint.Forehead,
			"eyes" => MaskPositionPoint.Eyes,
			"mouth" => MaskPositionPoint.Mouth,
			"chin" => MaskPositionPoint.Chin,
			_ => 0,
		};
}
