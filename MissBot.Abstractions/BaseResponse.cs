using System.Collections;
using MissBot.Abstractions.Converters;
using MissBot.Entities;
using MissBot.Entities.Abstractions;
using MissBot.Entities.Enums;

namespace MissBot.Abstractions
{
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public abstract record BaseResponse<TUnit>(IHandleContext Context = default) : BaseRequest<Message>("sendMessage"), IResponse<TUnit> where TUnit : class
	{
		[JsonProperty(Required = Required.Always)]
		public ChatId ChatId
			=> Chat?.Id ?? Context.Take<User>("from").Id;

		protected Chat Chat
			=> Context.Take<Chat>();

		/// <summary>
		/// Unique identifier for the target message thread (topic) of the forum; for forum supergroups only
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public int? MessageThreadId { get; set; }

		/// <summary>
		/// Text of the message to be sent, 1-4096 characters after entities parsing
		/// </summary>
		[JsonProperty("text", Required = Required.Always)]
		public abstract IUnitEntity Content { get; protected set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public string Caption { get; set; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public ParseMode? ParseMode { get; set; } = MissBot.Entities.Enums.ParseMode.Html;

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

		[JsonProperty("reply_markup", DefaultValueHandling = DefaultValueHandling.Ignore)]
		[JsonConverter(typeof(UnitActionsConverter), "inline_keyboard")]
		public IEnumerable<IEnumerable<IUnitAction<TUnit>>> Actions { get; set; }

		public virtual IActionsSet ActionsSet { get; set; }

		public abstract int Length { get; }

		public abstract IResponse<TUnit> InputDataInteraction(string description, IActionsSet options = null);
		public abstract IResponse CompleteInteraction(object completeObject);
		public abstract Task Commit(CancellationToken cancel = default);


		public virtual void AddUnits<TData>(IEnumerable<IUnit<TData>> units) where TData : class, TUnit
		{
			InitializeCombinedContent();
			foreach (var unit in units)
				AddUnit(unit);
		}

		public abstract void WriteUnit<TData>(TData unit) where TData : class, IUnit<TUnit>;
		public abstract void InitializeCombinedContent();
		public abstract IResponse Write<TData>(TData data);
		public abstract IResponse Write(IUnitItem data);
		public abstract void AddUnit<TData>(IUnit<TData> unit) where TData : class, TUnit;
		public abstract void WriteMetadata<TData>(TData meta) where TData : class, IMetaData;
		public abstract void AddUnits<TData>(IEnumerable<TData> units) where TData : class, IUnit<TUnit>;
		protected IEnumerator CountEnumerator { get; set; }
		public void SetCounter(IEnumerator countEnumerator)
		{
			CountEnumerator = countEnumerator;
		}
	}
}
