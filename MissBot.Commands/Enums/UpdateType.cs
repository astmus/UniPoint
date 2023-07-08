using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// The type of an <see cref="Update"/>
/// </summary>
[JsonConverter(typeof(UpdateTypeConverter))]
public enum UpdateType
{
	/// <summary>
	/// Update Type is unknown
	/// </summary>
	Unknown = 0,

	/// <summary>
	/// The <see cref="Update"/> contains a <see cref="Types.Message"/>.
	/// </summary>
	Message,

	/// <summary>
	/// The <see cref="Update"/> contains an <see cref="Types.InlineQuery"/>.
	/// </summary>
	InlineQuery,

	/// <summary>
	/// The <see cref="Update"/> contains a <see cref="Types.ChosenInlineResult"/>.
	/// </summary>
	ChosenInlineResult,

	/// <summary>
	/// The <see cref="Update"/> contains a <see cref="Types.CallbackQuery"/>
	/// </summary>
	CallbackQuery,

	/// <summary>
	/// The <see cref="Update"/> contains an edited <see cref="Types.Message"/>
	/// </summary>
	EditedMessage,

	/// <summary>
	/// The <see cref="Update"/> contains a channel post <see cref="Types.Message"/>
	/// </summary>
	ChannelPost,

	/// <summary>
	/// The <see cref="Update"/> contains an edited channel post <see cref="Types.Message"/>
	/// </summary>
	EditedChannelPost,

	/// <summary>
	/// The <see cref="Update"/> contains an <see cref="ShippingQuery"/>
	/// </summary>
	ShippingQuery,

	/// <summary>
	/// The <see cref="Update"/> contains an <see cref="PreCheckoutQuery"/>
	/// </summary>
	PreCheckoutQuery,

	/// <summary>
	/// The <see cref="Update"/> contains an <see cref="Poll"/>
	/// </summary>
	Poll,

	/// <summary>
	/// The <see cref="Update"/> contains an <see cref="PollAnswer"/>
	/// </summary>
	PollAnswer,

	/// <summary>
	/// The <see cref="Update"/> contains an <see cref="MyChatMember"/>
	/// </summary>
	MyChatMember,

	/// <summary>
	/// The <see cref="Update"/> contains an <see cref="ChatMember"/>
	/// </summary>
	ChatMember,

	/// <summary>
	/// The <see cref="Update"/> contains an <see cref="ChatJoinRequest"/>
	/// </summary>
	ChatJoinRequest,
}

internal partial class UpdateTypeConverter : JsonConverter<UpdateType>
{
	public override void WriteJson(JsonWriter writer, UpdateType value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			UpdateType.Unknown => "unknown",
			UpdateType.Message => "message",
			UpdateType.InlineQuery => "inline_query",
			UpdateType.ChosenInlineResult => "chosen_inline_result",
			UpdateType.CallbackQuery => "callback_query",
			UpdateType.EditedMessage => "edited_message",
			UpdateType.ChannelPost => "channel_post",
			UpdateType.EditedChannelPost => "edited_channel_post",
			UpdateType.ShippingQuery => "shipping_query",
			UpdateType.PreCheckoutQuery => "pre_checkout_query",
			UpdateType.Poll => "poll",
			UpdateType.PollAnswer => "poll_answer",
			UpdateType.MyChatMember => "my_chat_member",
			UpdateType.ChatMember => "chat_member",
			UpdateType.ChatJoinRequest => "chat_join_request",
			_ => throw new NotSupportedException(),
		});

	public override UpdateType ReadJson(JsonReader reader, Type objectType, UpdateType existingValue, bool hasExistingValue, JsonSerializer serializer) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"unknown" => UpdateType.Unknown,
			"message" => UpdateType.Message,
			"inline_query" => UpdateType.InlineQuery,
			"chosen_inline_result" => UpdateType.ChosenInlineResult,
			"callback_query" => UpdateType.CallbackQuery,
			"edited_message" => UpdateType.EditedMessage,
			"channel_post" => UpdateType.ChannelPost,
			"edited_channel_post" => UpdateType.EditedChannelPost,
			"shipping_query" => UpdateType.ShippingQuery,
			"pre_checkout_query" => UpdateType.PreCheckoutQuery,
			"poll" => UpdateType.Poll,
			"poll_answer" => UpdateType.PollAnswer,
			"my_chat_member" => UpdateType.MyChatMember,
			"chat_member" => UpdateType.ChatMember,
			"chat_join_request" => UpdateType.ChatJoinRequest,
			_ => UpdateType.Unknown,
		};
}
