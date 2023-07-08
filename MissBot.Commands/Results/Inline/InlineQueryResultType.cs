// ReSharper disable once CheckNamespace

using Newtonsoft.Json.Linq;

namespace MissBot.Entities.Results.Inline;

/// <summary>
/// Type of the InlineQueryResult
/// </summary>
[JsonConverter(typeof(InlineQueryResultTypeConverter))]
public enum InlineQueryResultType
{
	/// <summary>
	/// <see cref="InlineQueryResultArticle"/>
	/// </summary>
	Article = 1,

	/// <summary>
	/// <see cref="InlineQueryResultPhoto"/>
	/// <see cref="InlineQueryResultCachedPhoto"/>
	/// </summary>
	Photo,

	/// <summary>
	/// <see cref="InlineQueryResultGif"/>
	/// <see cref="InlineQueryResultCachedMpeg4Gif"/>
	/// </summary>
	Gif,

	/// <summary>
	/// <see cref="InlineQueryResultMpeg4Gif"/>
	/// <see cref="InlineQueryResultCachedVideo"/>
	/// </summary>
	Mpeg4Gif,

	/// <summary>
	/// <see cref="InlineQueryResultVideo"/>
	/// /// <see cref="InlineQueryResultCachedVideo"/>
	/// </summary>
	Video,

	/// <summary>
	/// <see cref="InlineQueryResultAudio"/>
	/// <see cref="InlineQueryResultCachedAudio"/>
	/// </summary>
	Audio,

	/// <summary>
	/// <see cref="InlineQueryResultContact"/>
	/// </summary>
	Contact,

	/// <summary>
	/// <see cref="InlineQueryResultDocument"/>
	/// /// <see cref="InlineQueryResultCachedDocument"/>
	/// </summary>
	Document,

	/// <summary>
	/// <see cref="InlineQueryResultLocation"/>
	/// </summary>
	Location,

	/// <summary>
	/// <see cref="InlineQueryResultVenue"/>
	/// </summary>
	Venue,

	/// <summary>
	/// <see cref="InlineQueryResultVoice"/>
	/// <see cref="InlineQueryResultCachedVoice"/>
	/// </summary>
	Voice,

	/// <summary>
	/// <see cref="InlineQueryResultGame"/>
	/// </summary>
	Game,

	/// <summary>
	/// <see cref="InlineQueryResultCachedSticker"/>
	/// </summary>
	Sticker,
}


internal partial class InlineQueryResultTypeConverter : JsonConverter<InlineQueryResultType>
{
	public override void WriteJson(JsonWriter writer, InlineQueryResultType value, JsonSerializer serializer) =>
		writer.WriteValue(value switch
		{
			InlineQueryResultType.Article => "article",
			InlineQueryResultType.Photo => "photo",
			InlineQueryResultType.Gif => "gif",
			InlineQueryResultType.Mpeg4Gif => "mpeg4_gif",
			InlineQueryResultType.Video => "video",
			InlineQueryResultType.Audio => "audio",
			InlineQueryResultType.Contact => "contact",
			InlineQueryResultType.Document => "document",
			InlineQueryResultType.Location => "location",
			InlineQueryResultType.Venue => "venue",
			InlineQueryResultType.Voice => "voice",
			InlineQueryResultType.Game => "game",
			InlineQueryResultType.Sticker => "sticker",
			(InlineQueryResultType)0 => "unknown",
			_ => throw new NotSupportedException(),
		});

	public override InlineQueryResultType ReadJson(
		JsonReader reader,
		Type objectType,
	InlineQueryResultType existingValue,
		bool hasExistingValue,
		JsonSerializer serializer
	) =>
		JToken.ReadFrom(reader).Value<string>() switch
		{
			"article" => InlineQueryResultType.Article,
			"photo" => InlineQueryResultType.Photo,
			"gif" => InlineQueryResultType.Gif,
			"mpeg4_gif" => InlineQueryResultType.Mpeg4Gif,
			"video" => InlineQueryResultType.Video,
			"audio" => InlineQueryResultType.Audio,
			"contact" => InlineQueryResultType.Contact,
			"document" => InlineQueryResultType.Document,
			"location" => InlineQueryResultType.Location,
			"venue" => InlineQueryResultType.Venue,
			"voice" => InlineQueryResultType.Voice,
			"game" => InlineQueryResultType.Game,
			"sticker" => InlineQueryResultType.Sticker,
			_ => 0,
		};
}
