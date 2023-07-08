using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Enums;

/// <summary>
/// Type of action to broadcast
/// </summary>
[JsonConverter(typeof(ChatActionConverter))]
public enum ChatAction
{
	/// <summary>
	/// Typing
	/// </summary>
	Typing = 1,

	/// <summary>
	/// Uploading a <see cref="PhotoSize"/>
	/// </summary>
	UploadPhoto,

	/// <summary>
	/// Recording a <see cref="Video"/>
	/// </summary>
	RecordVideo,

	/// <summary>
	/// Uploading a <see cref="Video"/>
	/// </summary>
	UploadVideo,

	/// <summary>
	/// Recording a <see cref="Voice"/>
	/// </summary>
	RecordVoice,

	/// <summary>
	/// Uploading a <see cref="Voice"/>
	/// </summary>
	UploadVoice,

	/// <summary>
	/// Uploading a <see cref="Document"/>
	/// </summary>
	UploadDocument,

	/// <summary>
	/// Finding a <see cref="Location"/>
	/// </summary>
	FindLocation,

	/// <summary>
	/// Recording a <see cref="VideoNote"/>
	/// </summary>
	RecordVideoNote,

	/// <summary>
	/// Uploading a <see cref="VideoNote"/>
	/// </summary>
	UploadVideoNote,

	/// <summary>
	/// Choosing a <see cref="Sticker"/>
	/// </summary>
	ChooseSticker,
}


internal partial class ChatActionConverter : JsonConverter<ChatAction>
{
	public override void WriteJson(JsonWriter writer, ChatAction value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			ChatAction.Typing => "typing",
			ChatAction.UploadPhoto => "upload_photo",
			ChatAction.RecordVideo => "record_video",
			ChatAction.UploadVideo => "upload_video",
			ChatAction.RecordVoice => "record_voice",
			ChatAction.UploadVoice => "upload_voice",
			ChatAction.UploadDocument => "upload_document",
			ChatAction.FindLocation => "find_location",
			ChatAction.RecordVideoNote => "record_video_note",
			ChatAction.UploadVideoNote => "upload_video_note",
			ChatAction.ChooseSticker => "choose_sticker",
			(ChatAction)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override ChatAction ReadJson(
		JsonReader reader,
		Type objectType,
	ChatAction existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"typing" => ChatAction.Typing,
			"upload_photo" => ChatAction.UploadPhoto,
			"record_video" => ChatAction.RecordVideo,
			"upload_video" => ChatAction.UploadVideo,
			"record_voice" => ChatAction.RecordVoice,
			"upload_voice" => ChatAction.UploadVoice,
			"upload_document" => ChatAction.UploadDocument,
			"find_location" => ChatAction.FindLocation,
			"record_video_note" => ChatAction.RecordVideoNote,
			"upload_video_note" => ChatAction.UploadVideoNote,
			"choose_sticker" => ChatAction.ChooseSticker,
			_ => 0,
		};
}
