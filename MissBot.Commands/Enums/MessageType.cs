using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// The type of a <see cref="Message"/>
/// </summary>
[JsonConverter(typeof(MessageTypeConverter))]
public enum MessageType
{
	/// <summary>
	/// The <see cref="Message"/> is unknown
	/// </summary>
	Unknown = 0,

	/// <summary>
	/// The <see cref="Message"/> contains text
	/// </summary>
	Text,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.PhotoSize"/>
	/// </summary>
	Photo,

	/// <summary>
	/// The <see cref="Message"/> contains an <see cref="Types.Audio"/>
	/// </summary>
	Audio,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.Video"/>
	/// </summary>
	Video,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.Voice"/>
	/// </summary>
	Voice,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.Document"/>
	/// </summary>
	Document,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.Sticker"/>
	/// </summary>
	Sticker,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.Location"/>
	/// </summary>
	Location,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.Contact"/>
	/// </summary>
	Contact,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.Venue"/>
	/// </summary>
	Venue,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.Game"/>
	/// </summary>
	Game,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.VideoNote"/>
	/// </summary>
	VideoNote,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.Payments.Invoice"/>
	/// </summary>
	Invoice,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Types.Payments.SuccessfulPayment"/>
	/// </summary>
	SuccessfulPayment,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Message.ConnectedWebsite"/>
	/// </summary>
	WebsiteConnected,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Message.NewChatMembers"/>
	/// </summary>
	ChatMembersAdded,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Message.LeftChatMember"/>
	/// </summary>
	ChatMemberLeft,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Message.NewChatTitle"/>
	/// </summary>
	ChatTitleChanged,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Message.NewChatPhoto"/>
	/// </summary>
	ChatPhotoChanged,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Message.PinnedMessage"/>
	/// </summary>
	MessagePinned,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Message.DeleteChatPhoto"/>
	/// </summary>
	ChatPhotoDeleted,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Message.GroupChatCreated"/>
	/// </summary>
	GroupCreated,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Message.SupergroupChatCreated"/>
	/// </summary>
	SupergroupCreated,

	/// <summary>
	/// The <see cref="Message"/> contains a <see cref="Message.ChannelChatCreated"/>
	/// </summary>
	ChannelCreated,

	/// <summary>
	/// The <see cref="Message"/> contains non-default <see cref="Message.MigrateFromChatId"/>
	/// </summary>
	MigratedToSupergroup,

	/// <summary>
	/// The <see cref="Message"/> contains non-default <see cref="Message.MigrateToChatId"/>
	/// </summary>
	MigratedFromGroup,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.Poll"/>
	/// </summary>
	Poll,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.Dice"/>
	/// </summary>
	Dice,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.MessageAutoDeleteTimerChanged"/>
	/// </summary>
	MessageAutoDeleteTimerChanged,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.ProximityAlertTriggered"/>
	/// </summary>
	ProximityAlertTriggered,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.WebAppData"/>
	/// </summary>
	WebAppData,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.VideoChatScheduled"/>
	/// </summary>
	VideoChatScheduled,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.VideoChatStarted"/>
	/// </summary>
	VideoChatStarted,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.VideoChatEnded"/>
	/// </summary>
	VideoChatEnded,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.VideoChatParticipantsInvited"/>
	/// </summary>
	VideoChatParticipantsInvited,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.Animation"/>
	/// </summary>
	Animation,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.ForumTopicCreated"/>
	/// </summary>
	ForumTopicCreated,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.ForumTopicClosed"/>
	/// </summary>
	ForumTopicClosed,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.ForumTopicReopened"/>
	/// </summary>
	ForumTopicReopened,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.ForumTopicEdited"/>
	/// </summary>
	ForumTopicEdited,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.GeneralForumTopicHidden"/>
	/// </summary>
	GeneralForumTopicHidden,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.GeneralForumTopicUnhidden"/>
	/// </summary>
	GeneralForumTopicUnhidden,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.WriteAccessAllowed"/>
	/// </summary>
	WriteAccessAllowed,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.UserShared"/>
	/// </summary>
	UserShared,

	/// <summary>
	/// The <see cref="Message"/> contains <see cref="Message.ChatShared"/>
	/// </summary>
	ChatShared,
}


internal partial class MessageTypeConverter : JsonConverter<MessageType>
{
	public override void WriteJson(JsonWriter writer, MessageType value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			MessageType.Unknown => "unknown",
			MessageType.Text => "text",
			MessageType.Photo => "photo",
			MessageType.Audio => "audio",
			MessageType.Video => "video",
			MessageType.Voice => "voice",
			MessageType.Document => "document",
			MessageType.Sticker => "sticker",
			MessageType.Location => "location",
			MessageType.Contact => "contact",
			MessageType.Venue => "venue",
			MessageType.Game => "game",
			MessageType.VideoNote => "video_note",
			MessageType.Invoice => "invoice",
			MessageType.SuccessfulPayment => "successful_payment",
			MessageType.WebsiteConnected => "website_connected",
			MessageType.ChatMembersAdded => "chat_members_added",
			MessageType.ChatMemberLeft => "chat_member_left",
			MessageType.ChatTitleChanged => "chat_title_changed",
			MessageType.ChatPhotoChanged => "chat_photo_changed",
			MessageType.MessagePinned => "message_pinned",
			MessageType.ChatPhotoDeleted => "chat_photo_deleted",
			MessageType.GroupCreated => "group_created",
			MessageType.SupergroupCreated => "supergroup_created",
			MessageType.ChannelCreated => "channel_created",
			MessageType.MigratedToSupergroup => "migrated_to_supergroup",
			MessageType.MigratedFromGroup => "migrated_from_group",
			MessageType.Poll => "poll",
			MessageType.Dice => "dice",
			MessageType.MessageAutoDeleteTimerChanged => "message_auto_delete_timer_changed",
			MessageType.ProximityAlertTriggered => "proximity_alert_triggered",
			MessageType.WebAppData => "web_app_data",
			MessageType.VideoChatScheduled => "video_chat_scheduled",
			MessageType.VideoChatStarted => "video_chat_started",
			MessageType.VideoChatEnded => "video_chat_ended",
			MessageType.VideoChatParticipantsInvited => "video_chat_participants_invited",
			MessageType.Animation => "animation",
			MessageType.ForumTopicCreated => "forum_topic_created",
			MessageType.ForumTopicClosed => "forum_topic_closed",
			MessageType.ForumTopicReopened => "forum_topic_reopened",
			MessageType.ForumTopicEdited => "forum_topic_edited",
			MessageType.GeneralForumTopicHidden => "general_forum_topic_hidden",
			MessageType.GeneralForumTopicUnhidden => "general_forum_topic_unhidden",
			MessageType.WriteAccessAllowed => "write_access_allowed",
			MessageType.UserShared => "user_shared",
			MessageType.ChatShared => "chat_shared",
			_ => throw new NotSupportedException(),
		});

	public override MessageType ReadJson(
		JsonReader reader,
		Type objectType,
	MessageType existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"unknown" => MessageType.Unknown,
			"text" => MessageType.Text,
			"photo" => MessageType.Photo,
			"audio" => MessageType.Audio,
			"video" => MessageType.Video,
			"voice" => MessageType.Voice,
			"document" => MessageType.Document,
			"sticker" => MessageType.Sticker,
			"location" => MessageType.Location,
			"contact" => MessageType.Contact,
			"venue" => MessageType.Venue,
			"game" => MessageType.Game,
			"video_note" => MessageType.VideoNote,
			"invoice" => MessageType.Invoice,
			"successful_payment" => MessageType.SuccessfulPayment,
			"website_connected" => MessageType.WebsiteConnected,
			"chat_members_added" => MessageType.ChatMembersAdded,
			"chat_member_left" => MessageType.ChatMemberLeft,
			"chat_title_changed" => MessageType.ChatTitleChanged,
			"chat_photo_changed" => MessageType.ChatPhotoChanged,
			"message_pinned" => MessageType.MessagePinned,
			"chat_photo_deleted" => MessageType.ChatPhotoDeleted,
			"group_created" => MessageType.GroupCreated,
			"supergroup_created" => MessageType.SupergroupCreated,
			"channel_created" => MessageType.ChannelCreated,
			"migrated_to_supergroup" => MessageType.MigratedToSupergroup,
			"migrated_from_group" => MessageType.MigratedFromGroup,
			"poll" => MessageType.Poll,
			"dice" => MessageType.Dice,
			"message_auto_delete_timer_changed" => MessageType.MessageAutoDeleteTimerChanged,
			"proximity_alert_triggered" => MessageType.ProximityAlertTriggered,
			"web_app_data" => MessageType.WebAppData,
			"video_chat_scheduled" => MessageType.VideoChatScheduled,
			"video_chat_started" => MessageType.VideoChatStarted,
			"video_chat_ended" => MessageType.VideoChatEnded,
			"video_chat_participants_invited" => MessageType.VideoChatParticipantsInvited,
			"animation" => MessageType.Animation,
			"forum_topic_created" => MessageType.ForumTopicCreated,
			"forum_topic_closed" => MessageType.ForumTopicClosed,
			"forum_topic_reopened" => MessageType.ForumTopicReopened,
			"forum_topic_edited" => MessageType.ForumTopicEdited,
			"general_forum_topic_hidden" => MessageType.GeneralForumTopicHidden,
			"general_forum_topic_unhidden" => MessageType.GeneralForumTopicUnhidden,
			"write_access_allowed" => MessageType.WriteAccessAllowed,
			"user_shared" => MessageType.UserShared,
			"chat_shared" => MessageType.ChatShared,
			_ => MessageType.Unknown,
		};
}
