using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// Type of the <see cref="Chat"/>, from which the inline query was sent
/// </summary>
[JsonConverter(typeof(ChatTypeConverter))]
public enum ChatType
{
	/// <summary>
	/// Normal one to one <see cref="Chat"/>
	/// </summary>
	Private = 1,

	/// <summary>
	/// Normal group chat
	/// </summary>
	Group,

	/// <summary>
	/// A channel
	/// </summary>
	Channel,

	/// <summary>
	/// A supergroup
	/// </summary>
	Supergroup,

	/// <summary>
	/// “sender” for a private chat with the inline query sender
	/// </summary>
	Sender
}


internal partial class ChatTypeConverter : JsonConverter<ChatType>
{
	public override void WriteJson(JsonWriter writer, ChatType value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			ChatType.Private => "private",
			ChatType.Group => "group",
			ChatType.Channel => "channel",
			ChatType.Supergroup => "supergroup",
			ChatType.Sender => "sender",
			(ChatType)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override ChatType ReadJson(
		JsonReader reader,
		Type objectType,
	ChatType existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"private" => ChatType.Private,
			"group" => ChatType.Group,
			"channel" => ChatType.Channel,
			"supergroup" => ChatType.Supergroup,
			"sender" => ChatType.Sender,
			_ => 0,
		};
}
