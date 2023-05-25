using System.Diagnostics.CodeAnalysis;
using MissBot.Abstractions.Actions;
using MissCore.Data.Entities;

[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class ChatActions : IActionsSet
{
    /// <summary>
    /// Optional. Use this parameter if you want to show the keyboard to specific users only. Targets:
    /// <list type="number">
    /// <item>
    /// users that are @mentioned in the <see cref="Message.Text"/> of the <see cref="Message"/> object;
    /// </item>
    /// <item>
    /// if the bot’s message is a reply (has <see cref="Message.ReplyToMessage"/>), sender of the original
    /// message.
    /// </item>
    /// </list>
    /// </summary>
    /// <remarks>
    /// <i>Example:</i> A user requests to change the bot’s language, bot replies to the request with a keyboard
    /// to select the new language. Other users in the group don't see the keyboard.
    /// </remarks>
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool? Selective { get; set; }

    [JsonProperty("keyboard",Required = Required.Always)]
    public IEnumerable<IEnumerable<ChatAction>> Actions { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool? IsPersistent { get; set; }
    
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool? ResizeKeyboard { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool? OneTimeKeyboard { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? InputFieldPlaceholder { get; set; }

    public ChatActions(ChatAction action) : this(new[] { action }) { }

    public ChatActions(IEnumerable<ChatAction> actionsRow) : this(new[] { actionsRow }) { }

    [JsonConstructor]
    public ChatActions(IEnumerable<IEnumerable<ChatAction>> actions)
    {
        Actions = actions;
    }
    public static ChatActions Create(params ChatAction[] actions)
        => new ChatActions(actions);

    public static implicit operator ChatActions?(string? text) =>
        text is null
            ? default
            : new(new[] { new ChatAction(text) });

    public static implicit operator ChatActions?(string[]? texts) =>
        texts is null
            ? default
            : new[] { texts };


    public static implicit operator ChatActions?(string[][]? textsItems) =>
        textsItems is null
            ? default
            : new ChatActions(
                textsItems.Select(texts =>
                    texts.Select(t => new ChatAction(t))
                ));
}
