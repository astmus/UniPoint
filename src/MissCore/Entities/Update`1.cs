using MissBot.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MissCore.Entities
{
    public class Update<TEntity> : Update, IUpdateInfo, IUpdate<TEntity>, ICommonUpdate
    {
        public string Text
            => Message.Text;
        public Chat Chat
        => Message.Chat;

        public new Message Message
            => base.Message ?? EditedMessage ?? ChannelPost ?? EditedChannelPost;

        public IResponse Response { get; internal set; }
        public TEntity Data { get; set; }
        public bool IsCommand => this switch
        {
            { Message: { } } when Message.Entities.Any(a => a.Type == MessageEntityType.BotCommand) => true,            
            _ => false
        };
    }
}
