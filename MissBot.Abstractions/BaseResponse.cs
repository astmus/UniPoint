using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using MissBot.Entities;

namespace MissBot.Abstractions
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public abstract record BaseResponse<TResponse>(IHandleContext Context = default) : BaseRequest<Message<TResponse>>("sendMessage"), IResponse<TResponse>, IUnitContainable<TResponse> where TResponse:BaseUnit, IUnit, IBotEntity
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
        public virtual IUnit<TResponse> Content { get; set; }
        public abstract int Length { get; }

    
        public abstract IResponse<TResponse> InputData(string description, IActionsSet options = null);
        public abstract IResponse CompleteInput(string message);
        public abstract Task Commit(CancellationToken cancel = default);

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public string Caption { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public Telegram.Bot.Types.Enums.ParseMode? ParseMode { get; set; } = Telegram.Bot.Types.Enums.ParseMode.Html;

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<MessageEntity> Entities { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableWebPagePreview { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableNotification { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? ProtectContent { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int? ReplyToMessageId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? AllowSendingWithoutReply { get; set; }

        [JsonProperty("reply_markup",DefaultValueHandling = DefaultValueHandling.Ignore)]
        public virtual IActionsSet Actions { get; }

        void IResponse<TResponse>.WriteMetadata<TData>(TData meta)
        {
            throw new NotImplementedException();
        }

        public void Write<TData>(TData unit) where TData : IUnit<TResponse>//IUnit<TResponse>
        {
            Content = unit;
        }

        public void Write<TData>(IEnumerable<TData> units) where TData:TResponse
        {
            //foreach (var unit in units)
            //    Write(unit);
        }

        public void Add<TUnit>(TUnit unit) where TUnit : IUnit<TResponse>
        {
            throw new NotImplementedException();
        }
    }
}
