using MissBot.Entities;
using MissBot.Entities.Abstractions;
using MissBot.Entities.Enums;

namespace MissBot.Abstractions
{
	/// <summary>
	/// Use this method to edit text and game messages. On success the edited <see cref="Message"/> is returned.
	/// </summary>
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
	public record UpdateDataRequest<TEntity> : BaseRequest<TEntity>
	{
		/// <inheritdoc />
		[JsonProperty(Required = Required.Always)]
		public ChatId ChatId { get; }

		/// <summary>
		/// Identifier of the message to edit
		/// </summary>
		[JsonProperty(Required = Required.Always)]
		public int MessageId { get; }

		/// <summary>
		/// New text of the message, 1-4096 characters after entities parsing
		/// </summary>
		[JsonProperty(Required = Required.Always)]
		public string Text { get; }

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public ParseMode ParseMode { get; set; } = ParseMode.Html;

		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IEnumerable<MessageEntity> Entities { get; set; }

		/// <summary>
		/// Disables link previews for links in this message
		/// </summary>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public bool DisableWebPagePreview { get; set; }

		/// <inheritdoc cref="Documentation.ReplyMarkup"/>
		[JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
		public IActionsSet ReplyMarkup { get; set; }

		/// <summary>
		/// Initializes a new request with chatId, messageId and text
		/// </summary>
		/// <param name="chatId">
		/// Unique identifier for the target chat or username of the target channel
		/// (in the format <c>@channelusername</c>)
		/// </param>
		/// <param name="messageId">Identifier of the message to edit</param>
		/// <param name="text">New text of the message, 1-4096 characters after entities parsing</param>
		public UpdateDataRequest(ChatId chatId, int messageId, TEntity data = default)
			: base("editMessageText")
		{
			ChatId = chatId;
			MessageId = messageId;
			Text = data.ToString();// .Select(d=> d.ToString()).SelectMany(sm=> sm+Environment.NewLine).ToString();
		}
	}
}
