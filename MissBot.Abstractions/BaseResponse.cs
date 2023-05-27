using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using MissBot.Entities;

namespace MissBot.Abstractions
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract record BaseResponse<TResponse>(IHandleContext Context = default) : BaseRequest<Message<TResponse>>("sendMessage"), IResponse<TResponse>
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
        public abstract int Length { get; }

        public abstract void Write<TData>(TData unit) where TData : UnitBase, IUnit<TResponse>;
        public abstract void WriteResult<TData>(TData unit) where TData : IEnumerable<IUnit<TResponse>>;
        public abstract void Write<TData>(IEnumerable<TData> units) where TData : UnitBase, IUnit<TResponse>;
        public abstract IResponse<TResponse> InputData(string description, IActionsSet options = null);
        public abstract IResponse CompleteInput(string message);
        public abstract Task Commit(CancellationToken cancel = default);

        void IResponse<TResponse>.WriteMetadata<TData>(TData meta)
        {
            throw new NotImplementedException();
        }
    }
}
