using MissBot.Abstractions.Actions;

namespace MissCore.Data.Entities;


[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
public class UnitAction : IUnitAction 
{
    [JsonProperty("text", Required = Required.Always)]
    public string ActionName { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? Url { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? CallbackData { get; set; }

    //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    //public WebAppInfo? WebApp { get; set; }

    //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    //public LoginUrl? LoginUrl { get; set; }
    
    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? SwitchInlineQuery { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public string? SwitchInlineQueryCurrentChat { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public SwitchInlineQueryChosenChat? SwitchInlineQueryChosenChat { get; set; }

    //[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    //public CallbackGame? CallbackGame { get; set; }

    [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
    public bool? Pay { get; set; }

    [JsonConstructor]
    public UnitAction(string text)
        =>  ActionName = text;    

    public static UnitAction WithUrl(string text, string url) =>
        new(text) { Url = url };

    //public static InlineUnitAction WithLoginUrl(string text, LoginUrl loginUrl) =>
    //    new(text) { LoginUrl = loginUrl };

    public static UnitAction WithCallbackData(string textAndCallbackData) =>
        new(textAndCallbackData) { CallbackData = textAndCallbackData };

    public static UnitAction WithCallbackData(string text, string callbackData) =>
        new(text) { CallbackData = callbackData };

    public static UnitAction WithSwitchInlineQuery(string text, string query = "") =>
        new(text) { SwitchInlineQuery = query };

    public static UnitAction WithSwitchInlineQueryCurrentChat(string text, string query = "") =>
        new(text) { SwitchInlineQueryCurrentChat = query };

    public static UnitAction WithSwitchInlineQueryChosenChat(string text, SwitchInlineQueryChosenChat switchInlineQueryChosenChat) =>
        new(text) { SwitchInlineQueryChosenChat = switchInlineQueryChosenChat };

    //public static InlineUnitAction WithCallBackGame(string text, CallbackGame? callbackGame = default) =>
    //    new(text) { CallbackGame = callbackGame ?? new() };

    public static UnitAction WithPayment(string text) =>
        new(text) { Pay = true };

    //public static InlineUnitAction WithWebApp(string text, WebAppInfo webAppInfo) =>
    //    new(text) { WebApp = webAppInfo };

    public static implicit operator UnitAction?(string? textAndCallbackData) =>
        textAndCallbackData is null
            ? default
            : WithCallbackData(textAndCallbackData);
}
