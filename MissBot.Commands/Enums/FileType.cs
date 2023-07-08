

using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// Type of a <see cref="InputFile"/>
/// </summary>
[JsonConverter(typeof(FileTypeConverter))]
public enum FileType
{
	/// <summary>
	/// FileStream
	/// </summary>
	Stream = 1,

	/// <summary>
	/// FileId
	/// </summary>
	Id,

	/// <summary>
	/// File URL
	/// </summary>
	Url
}


internal partial class FileTypeConverter : JsonConverter<FileType>
{
	public override void WriteJson(JsonWriter writer, FileType value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			FileType.Stream => "stream",
			FileType.Id => "id",
			FileType.Url => "url",
			(FileType)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override FileType ReadJson(
		JsonReader reader,
		Type objectType,
	FileType existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"stream" => FileType.Stream,
			"id" => FileType.Id,
			"url" => FileType.Url,
			_ => 0,
		};
}
