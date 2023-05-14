using MissBot.Entities;
/// <summary>
/// This object represents a message.
/// </summary>
namespace MissBot.Abstractions
{
    public class Message<TEntity> : Message
    {
        public UpdateDataRequest<TEntity> Update()
            => new UpdateDataRequest<TEntity>(Chat.Id, MessageId, Result);

        public TEntity Result { get; set; }
    }
}
