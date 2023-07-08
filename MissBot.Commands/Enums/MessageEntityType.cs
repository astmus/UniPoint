using Newtonsoft.Json.Linq;
using Telegram.Bot.Types;

namespace MissBot.Entities.Enums;

/// <summary>
/// Type of a <see cref="MessageEntity"/>
/// </summary>
[JsonConverter(typeof(MessageEntityTypeConverter))]
public enum MessageEntityType
{
	/// <summary>
	/// A mentioned <see cref="User"/>
	/// </summary>
	Mention = 1,

	/// <summary>
	/// A searchable Hashtag
	/// </summary>
	Hashtag,

	/// <summary>
	/// A Bot command
	/// </summary>
	BotCommand,

	/// <summary>
	/// An URL
	/// </summary>
	Url,

	/// <summary>
	/// An email
	/// </summary>
	Email,

	/// <summary>
	/// Bold text
	/// </summary>
	Bold,

	/// <summary>
	/// Italic text
	/// </summary>
	Italic,

	/// <summary>
	/// Monowidth string
	/// </summary>
	Code,

	/// <summary>
	/// Monowidth block
	/// </summary>
	Pre,

	/// <summary>
	/// Clickable text URLs
	/// </summary>
	TextLink,

	/// <summary>
	/// Mentions for a <see cref="User"/> without <see cref="User.Username"/>
	/// </summary>
	TextMention,

	/// <summary>
	/// Phone number
	/// </summary>
	PhoneNumber,

	/// <summary>
	/// A cashtag (e.g. $EUR, $USD) - $ followed by the short currency code
	/// </summary>
	Cashtag,

	/// <summary>
	/// Underlined text
	/// </summary>
	Underline,

	/// <summary>
	/// Strikethrough text
	/// </summary>
	Strikethrough,

	/// <summary>
	/// Spoiler message
	/// </summary>
	Spoiler,

	/// <summary>
	/// Inline custom emoji stickers
	/// </summary>
	CustomEmoji,
}


internal partial class MessageEntityTypeConverter : JsonConverter<MessageEntityType>
{
	public override void WriteJson(JsonWriter writer, MessageEntityType value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			MessageEntityType.Mention => "mention",
			MessageEntityType.Hashtag => "hashtag",
			MessageEntityType.BotCommand => "bot_command",
			MessageEntityType.Url => "url",
			MessageEntityType.Email => "email",
			MessageEntityType.Bold => "bold",
			MessageEntityType.Italic => "italic",
			MessageEntityType.Code => "code",
			MessageEntityType.Pre => "pre",
			MessageEntityType.TextLink => "text_link",
			MessageEntityType.TextMention => "text_mention",
			MessageEntityType.PhoneNumber => "phone_number",
			MessageEntityType.Cashtag => "cashtag",
			MessageEntityType.Underline => "underline",
			MessageEntityType.Strikethrough => "strikethrough",
			MessageEntityType.Spoiler => "spoiler",
			MessageEntityType.CustomEmoji => "custom_emoji",
			(MessageEntityType)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override MessageEntityType ReadJson(
		JsonReader reader,
		Type objectType,
	MessageEntityType existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"mention" => MessageEntityType.Mention,
			"hashtag" => MessageEntityType.Hashtag,
			"bot_command" => MessageEntityType.BotCommand,
			"url" => MessageEntityType.Url,
			"email" => MessageEntityType.Email,
			"bold" => MessageEntityType.Bold,
			"italic" => MessageEntityType.Italic,
			"code" => MessageEntityType.Code,
			"pre" => MessageEntityType.Pre,
			"text_link" => MessageEntityType.TextLink,
			"text_mention" => MessageEntityType.TextMention,
			"phone_number" => MessageEntityType.PhoneNumber,
			"cashtag" => MessageEntityType.Cashtag,
			"underline" => MessageEntityType.Underline,
			"strikethrough" => MessageEntityType.Strikethrough,
			"spoiler" => MessageEntityType.Spoiler,
			"custom_emoji" => MessageEntityType.CustomEmoji,
			_ => 0,
		};
}
