using LinqToDB.Mapping;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Bot;

namespace MissCore.Actions;


[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
[Table("##BotUnits")]
public record ChatAction : BaseBotAction
{
    [JsonProperty("text", Required = Required.Always)]
    //[Column("Entity")]
    public override string Action { get => base.Action; }

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
    /// <summary>
    /// Optional. If specified, pressing the button will open a list of suitable users. Tapping on any user will send
    /// their identifier to the bot in a “user_shared” service message. Available in private chats only.
    /// </summary>
    //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    //public KeyboardButtonRequestUser? RequestUser { get; set; }

    /// <summary>
    /// Optional. If specified, pressing the button will open a list of suitable chats. Tapping on a chat will send
    /// its identifier to the bot in a “chat_shared” service message. Available in private chats only.
    /// </summary>
    //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    //public KeyboardButtonRequestChat? RequestChat { get; set; }

    /// <summary>
    /// Optional. If <see langword="true"/>, the user's phone number will be sent as a contact when the button
    /// is pressed. Available in private chats only
    /// </summary>
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool? RequestContact { get; set; }

    /// <summary>
    /// Optional. If <see langword="true"/>, the user's current location will be sent when the button is pressed.
    /// Available in private chats only
    /// </summary>
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool? RequestLocation { get; set; }
    [Column]
    public override string Unit { get; set; }

    /// <summary>
    /// Optional. If specified, the user will be asked to create a poll and send it to the bot when the button
    /// is pressed. Available in private chats only
    /// </summary>
    //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    //public KeyboardButtonPollType? RequestPoll { get; set; }

    /// <summary>
    /// Optional. If specified, the described Web App will be launched when the button is pressed. The Web App will
    /// be able to send a “web_app_data” service message. Available in private chats only.
    /// </summary>
    //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    //public WebAppInfo? WebApp { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="KeyboardButton"/> class.
    /// </summary>
    /// <param name="actoinTitle">Label text on the button</param>
    [JsonConstructor]
    public ChatAction(string actoinTitle) => Action = actoinTitle;
    public ChatAction() { }
    /// <summary>
    /// Generate a keyboard button to request for contact
    /// </summary>
    /// <param name="text">Button's text</param>
    /// <returns>Keyboard button</returns>
    public static ChatAction WithRequestContact(string text) =>
        new(text) { RequestContact = true };

    /// <summary>
    /// Generate a keyboard button to request for location
    /// </summary>
    /// <param name="text">Button's text</param>
    /// <returns>Keyboard button</returns>
    public static ChatAction WithRequestLocation(string text) =>
        new(text) { RequestLocation = true };

    /// <summary>
    /// Generate a keyboard button to request a poll
    /// </summary>
    /// <param name="text">Button's text</param>
    /// <param name="type">Poll's type</param>
    /// <returns>Keyboard button</returns>
    //public static ChatAction WithRequestPoll(string text, string? type = default) =>
    //    new(text) { RequestPoll = new() { Type = type } };

    /// <summary>
    /// Generate a keyboard button to request a web app
    /// </summary>
    /// <param name="text">Button's text</param>
    /// <param name="webAppInfo">Web app information</param>
    /// <returns></returns>
    //public static ChatAction WithWebApp(string text, WebAppInfo webAppInfo) =>
    //    new(text) { WebApp = webAppInfo };

    /// <summary>
    /// Generate a keyboard button to request user info
    /// </summary>
    /// <param name="text">Button's text</param>
    /// <param name="requestUser">Criteria used to request a suitable user</param>
    /// <returns></returns>
    //public static ChatAction WithRequestUser(string text, KeyboardButtonRequestUser requestUser) =>
    //    new(text) { RequestUser = requestUser };

    /// <summary>
    /// Generate a keyboard button to request chat info
    /// </summary>
    /// <param name="text">Button's text</param>
    /// <param name="requestChat">Criteria used to request a suitable chat</param>
    /// <returns></returns>
    //public static ChatAction WithRequestChat(string text, KeyboardButtonRequestChat requestChat) =>
    //    new(text) { RequestChat = requestChat };

    /// <summary>
    /// Generate a keyboard button from text
    /// </summary>
    /// <param name="text">Button's text</param>
    /// <returns>Keyboard button</returns>
    public static implicit operator ChatAction(string text)
        => new(text);
}
