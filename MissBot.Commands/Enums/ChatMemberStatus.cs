using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// ChatMember status
/// </summary>
[JsonConverter(typeof(ChatMemberStatusConverter))]
public enum ChatMemberStatus
{
	/// <summary>
	/// Creator of the <see cref="Chat"/>
	/// </summary>
	Creator = 1,

	/// <summary>
	/// Administrator of the <see cref="Chat"/>
	/// </summary>
	Administrator,

	/// <summary>
	/// Normal member of the <see cref="Chat"/>
	/// </summary>
	Member,

	/// <summary>
	/// A <see cref="User"/> who left the <see cref="Chat"/>
	/// </summary>
	Left,

	/// <summary>
	/// A <see cref="User"/> who was kicked from the <see cref="Chat"/>
	/// </summary>
	Kicked,

	/// <summary>
	/// A <see cref="User"/> who is restricted in the <see cref="Chat"/>
	/// </summary>
	Restricted
}


internal partial class ChatMemberStatusConverter : JsonConverter<ChatMemberStatus>
{
	public override void WriteJson(JsonWriter writer, ChatMemberStatus value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			ChatMemberStatus.Creator => "creator",
			ChatMemberStatus.Administrator => "administrator",
			ChatMemberStatus.Member => "member",
			ChatMemberStatus.Left => "left",
			ChatMemberStatus.Kicked => "kicked",
			ChatMemberStatus.Restricted => "restricted",
			(ChatMemberStatus)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override ChatMemberStatus ReadJson(
		JsonReader reader,
		Type objectType,
	ChatMemberStatus existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"creator" => ChatMemberStatus.Creator,
			"administrator" => ChatMemberStatus.Administrator,
			"member" => ChatMemberStatus.Member,
			"left" => ChatMemberStatus.Left,
			"kicked" => ChatMemberStatus.Kicked,
			"restricted" => ChatMemberStatus.Restricted,
			_ => 0,
		};
}
