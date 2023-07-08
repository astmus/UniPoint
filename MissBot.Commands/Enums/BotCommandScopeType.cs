using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// Scope type
/// </summary>
[JsonConverter(typeof(BotCommandScopeTypeConverter))]
public enum BotCommandScopeType
{
	/// <summary>
	/// Represents the default <see cref="BotCommandScope"/> of bot commands. Default commands are used if no
	/// commands with a narrower <see cref="BotCommandScope"/> are specified for the user.
	/// </summary>
	Default = 1,

	/// <summary>
	/// Represents the <see cref="BotCommandScope"/> of bot commands, covering all private chats.
	/// </summary>
	AllPrivateChats,

	/// <summary>
	/// Represents the <see cref="BotCommandScope"/> of bot commands, covering all group and supergroup chats.
	/// </summary>
	AllGroupChats,

	/// <summary>
	/// Represents the <see cref="BotCommandScope"/> of bot commands, covering all group and supergroup
	/// chat administrators.
	/// </summary>
	AllChatAdministrators,

	/// <summary>
	/// Represents the <see cref="BotCommandScope"/> of bot commands, covering a specific <see cref="Chat"/>.
	/// </summary>
	Chat,

	/// <summary>
	/// Represents the <see cref="BotCommandScope"/> of bot commands, covering all administrators of
	/// a specific group or supergroup <see cref="Chat"/>.
	/// </summary>
	ChatAdministrators,

	/// <summary>
	/// Represents the <see cref="BotCommandScope"/> of bot commands, covering a specific member of
	/// a group or supergroup <see cref="Chat"/>.
	/// </summary>
	ChatMember
}

internal partial class BotCommandScopeTypeConverter : JsonConverter<BotCommandScopeType>
{
	public override void WriteJson(JsonWriter writer, BotCommandScopeType value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			BotCommandScopeType.Default => "default",
			BotCommandScopeType.AllPrivateChats => "all_private_chats",
			BotCommandScopeType.AllGroupChats => "all_group_chats",
			BotCommandScopeType.AllChatAdministrators => "all_chat_administrators",
			BotCommandScopeType.Chat => "chat",
			BotCommandScopeType.ChatAdministrators => "chat_administrators",
			BotCommandScopeType.ChatMember => "chat_member",
			(BotCommandScopeType)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override BotCommandScopeType ReadJson(
		JsonReader reader,
		Type objectType,
	BotCommandScopeType existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"default" => BotCommandScopeType.Default,
			"all_private_chats" => BotCommandScopeType.AllPrivateChats,
			"all_group_chats" => BotCommandScopeType.AllGroupChats,
			"all_chat_administrators" => BotCommandScopeType.AllChatAdministrators,
			"chat" => BotCommandScopeType.Chat,
			"chat_administrators" => BotCommandScopeType.ChatAdministrators,
			"chat_member" => BotCommandScopeType.ChatMember,
			_ => 0,
		};
}
