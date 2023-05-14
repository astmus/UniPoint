using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

// ReSharper disable once CheckNamespace
namespace MissBot.Entities.Results.Inline;

/// <summary>
/// Base Class for inline results send in response to an <see cref="InlineQuery"/>
/// </summary>
[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public abstract class InlineQueryResult
{
    /// <summary>
    /// Type of the result
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public abstract InlineQueryResultType Type { get; }

    /// <summary>
    /// Unique identifier for this result, 1-64 Bytes
    /// </summary>
    [JsonProperty(Required = Required.Always)]
    public string Id { get; set; }
    /// <summary>
    /// Optional. Inline keyboard attached to the message
    /// </summary>
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public InlineKeyboardMarkup ReplyMarkup { get; set; }

    /// <summary>
    /// Initializes a new inline query result
    /// </summary>
    /// <param name="id">Unique identifier for this result, 1-64 Bytes</param>
    protected InlineQueryResult(string id) => Id = id;
}

//[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
//public abstract record InlineUnit
//{
//    /// <summary>
//    /// Type of the result
//    /// </summary>
//    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
//    public virtual InlineQueryResultType Type { get; } = InlineQueryResultType.Article;

//    /// <summary>
//    /// Content of the message to be sent
//    /// </summary>
//    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
//    public abstract IInlineContent InputMessageContent { get; }
//    /// <summary>
//    /// Unique identifier for this result, 1-64 Bytes
//    /// </summary>
//    [JsonProperty(Required = Required.Always)]
//    public string Id { get; set; }
//    /// <summary>
//    /// Optional. Inline keyboard attached to the message
//    /// </summary>
//    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
//    public InlineKeyboardMarkup ReplyMarkup { get; set; }
//}

