

// ReSharper disable once CheckNamespace
using Telegram.Bot.Types;

namespace MissBot.Entities.Results.Inline;

/// <summary>
/// This object represents the content of a message to be sent as a result of an
/// <see cref="InlineQuery">inline query</see>.
/// </summary>
[JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public abstract class InputMessageContent { }
