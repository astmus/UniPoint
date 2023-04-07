using System.Threading.Tasks;
using Newtonsoft.Json;
using TG = Telegram.Bot.Types.Message;
/// <summary>
/// This object represents a message.
/// </summary>
namespace MissBot.Abstractions
{
    public class Message<TEntity> : TG
    {
        public UpdateDataRequest<TEntity> Update()
            => new UpdateDataRequest<TEntity>(Chat.Id, MessageId, Result);

        [JsonProperty(Required = Required.Always)]
        public new Chat Chat { get; set; }
        public TEntity Result { get; set; }
    }
}
