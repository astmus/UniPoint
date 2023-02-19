using TG = Telegram.Bot.Types.Message;
/// <summary>
/// This object represents a message.
/// </summary>
namespace MissBot.Commands
{
    public class Message<TEntity> : TG
    {
        public TEntity Data { get; set; }
    }
}
