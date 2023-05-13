using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissBot.Abstractions
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract record ResponseMessage<TResponse> : BaseRequest<Message<TResponse>>
    {
        [JsonIgnore]
        public Message<TResponse> Result { get; protected set; }
        /// <inheritdoc />
        [JsonProperty(Required = Required.Always)]
        public abstract ChatId ChatId { get; }

        /// <summary>
        /// Unique identifier for the target message thread (topic) of the forum; for forum supergroups only
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? MessageThreadId { get; set; }

        /// <summary>
        /// Text of the message to be sent, 1-4096 characters after entities parsing
        /// </summary>
        [JsonProperty(Required = Required.Always)]
        public string Text { get;  set; }

        /// <inheritdoc cref="Abstractions.Documentation.ParseMode"/>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode? ParseMode { get; set; } = Telegram.Bot.Types.Enums.ParseMode.Html;

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
        public ResponseMessage(string text = default) : base("sendMessage")
        {
            Text = text ?? "\n";
        }
    }
}
