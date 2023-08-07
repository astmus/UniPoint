

using MissBot.Abstractions;
using MissBot.Entities;
using MissBot.Entities.Enums;

namespace MissCore.Data
{
	public class Update<TEntity> : UnitUpdate, IUpdateInfo, IUpdate<TEntity>
	{
		public TEntity Data { get; set; }

	}
	public class UnitUpdate : Update, IUpdateInfo, IUnitUpdate
	{
		public string StringContent
				=> CurrentMessage?.Text;
		public Chat Chat
			=> CurrentMessage?.Chat ?? new Chat() { Id = InlineQuery?.From.Id ?? CallbackQuery?.From.Id ?? ChosenInlineResult.From.Id };

		public Message CurrentMessage
			=> Message ?? EditedMessage ?? ChannelPost ?? EditedChannelPost;

		public bool IsCommand => this switch
		{
			{ Message: { } } when CurrentMessage.Entities is MessageEntity[] ent && ent.Any(a => a.Type == MessageEntityType.BotCommand) => true,
			_ => false
		};

		public uint UpdateId => Convert.ToUInt32(Id);


	}
}
