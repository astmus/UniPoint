using MissBot.Abstractions;
using MissBot.Entities;
using Telegram.Bot.Types.Enums;

namespace MissCore.Data
{
    public class Update<TEntity> : Update, IUpdateInfo, IUpdate<TEntity>, ICommonUpdate
    {
        public string Text
            => Message.Text;
        public Chat Chat
            => Message?.Chat ?? new Chat() { Id = InlineQuery?.From.Id ?? CallbackQuery?.From.Id ?? ChosenInlineResult.From.Id };

        public new Message Message
            => base.Message ?? EditedMessage ?? ChannelPost ?? EditedChannelPost;

        public TEntity Data { get; set; }
        public bool IsCommand => this switch
        {
            { Message: { } } when Message.Entities is MessageEntity[] ent && ent.Any(a => a.Type == MessageEntityType.BotCommand) => true,
            _ => false
        };


        public uint UpdateId { get; }
        public bool? IsHandled { get; set; }
    }
}
