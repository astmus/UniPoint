using MissBot.Abstractions.Actions;
using MissBot.Entities;

namespace MissBot.Abstractions
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract record BaseResponse<TResponse>(IHandleContext Context = default) : BaseRequest<Message<TResponse>>("sendMessage")
    {
        /// <inheritdoc />
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId
            => Chat.Id;
        protected Chat Chat
            => Context.Take<Chat>() ?? Context.Any<IUnitUpdate>().Chat;
        /// <summary>
        /// Unique identifier for the target message thread (topic) of the forum; for forum supergroups only
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? MessageThreadId { get; set; }

        /// <summary>
        /// Text of the message to be sent, 1-4096 characters after entities parsing
        /// </summary>
        [JsonProperty("text", Required = Required.Always)]
        public string Content { get; set; }

        /// <summary>
        /// New caption of the message, 0-1024 characters after entities parsing
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Caption { get; set; }

        /// <inheritdoc cref="Abstractions.Documentation.ParseMode"/>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Telegram.Bot.Types.Enums.ParseMode? ParseMode { get; set; } = Telegram.Bot.Types.Enums.ParseMode.Html;

        /// <inheritdoc cref="Abstractions.Documentation.Entities"/>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<MessageEntity> Entities { get; set; }

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
        [JsonProperty("reply_markup",DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IActionsSet Actions { get; set; }
        }
}
