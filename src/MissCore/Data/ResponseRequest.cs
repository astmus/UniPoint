using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Entities;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissCore.Data
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record ResponseRequest<TResponse> : BaseRequest<Message<TResponse>> where TResponse : IResponse
    {
        [JsonIgnore]
        public TResponse ResponseData { get; set; }
        /// <inheritdoc />
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        /// <summary>
        /// Unique identifier for the target message thread (topic) of the forum; for forum supergroups only
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? MessageThreadId { get; set; }

        /// <summary>
        /// Text of the message to be sent, 1-4096 characters after entities parsing
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Text { get; }

        /// <inheritdoc cref="Abstractions.Documentation.ParseMode"/>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode? ParseMode { get; set; }

        /// <inheritdoc cref="Abstractions.Documentation.Entities"/>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<MessageEntity> Entities { get; set; }

        /// <summary>
        /// Disables link previews for links in this message
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableWebPagePreview { get; set; }

        /// <inheritdoc cref="Abstractions.Documentation.DisableNotification"/>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableNotification { get; set; }

        /// <inheritdoc cref="Abstractions.Documentation.ProtectContent"/>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ProtectContent { get; set; }

        /// <inheritdoc cref="Abstractions.Documentation.ReplyToMessageId"/>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ReplyToMessageId { get; set; }

        /// <inheritdoc cref="Abstractions.Documentation.AllowSendingWithoutReply"/>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? AllowSendingWithoutReply { get; set; }

        /// <inheritdoc cref="Abstractions.Documentation.ReplyMarkup"/>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReplyMarkup ReplyMarkup { get; set; }

        /// <summary>
        /// Initializes a new request with chatId and text
        /// </summary>
        /// <param name="chatId">Unique identifier for the target chat or username of the target channel
        /// (in the format <c>@channelusername</c>)
        /// </param>
        /// <param name="text">Text of the message to be sent, 1-4096 characters after entities parsing</param>
        public ResponseRequest(ChatId chatId)
            : base("sendMessage")
        {
            ChatId = chatId;
            Text = nameof(TResponse);
        }
    }
}
