
using MissBot.Entities;

namespace MissBot.Abstractions.Response
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public class AnswerCallbackQueryRequest : RequestBase<bool>
    {
        /// <summary>
        /// Unique identifier for the query to be answered
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public virtual string CallbackQueryId { get; }

        /// <summary>
        /// Text of the notification. If not specified, nothing will be shown to the user, 0-200 characters
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Text { get; set; }

        /// <summary>
        /// If true, an alert will be shown by the client instead of a notification at the top of
        /// the chat screen. Defaults to <see langword="false"/>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool ShowAlert { get; set; }

        /// <summary>
        /// URL that will be opened by the user's client. If you have created a
        /// <a href="https://core.telegram.org/bots/api#game">Game</a> and accepted the conditions
        /// via <c>@Botfather</c>, specify the URL that opens your game — note that this will only work
        /// if the query comes from a callback_game button.
        /// <para>
        /// Otherwise, you may use links like <c>t.me/your_bot start = XXXX</c> that open your bot with
        /// a parameter
        /// </para>
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Url { get; set; }

        /// <summary>
        /// The maximum amount of time in seconds that the result of the callback query may be cached
        /// client-side. Telegram apps will support caching starting in version 3.14. Defaults to 0
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int CacheTime { get; set; }

        /// <summary>
        /// Initializes a new request with callbackQueryId
        /// </summary>
        /// <param name="callbackQueryId">Unique identifier for the query to be answered</param>
        public AnswerCallbackQueryRequest(string callbackQueryId)
            : base("answerCallbackQuery")
        {
            CallbackQueryId = callbackQueryId;
        }
    }
}
