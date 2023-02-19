using MissBot.Abstractions;
using Telegram.Bot.Types;

namespace MissCore.Entities
{
    public class Update<TEntity> : Update, IUpdateInfo
    {
        public string Text
            => Message?.Text;
        public Chat Chat
        => Message?.Chat ?? EditedMessage?.Chat;
        public IResponseChannel Response { get; internal set; }
    }
}
