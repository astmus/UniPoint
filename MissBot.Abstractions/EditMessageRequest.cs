using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;


namespace MissBot.Abstractions;

//public class MessageRequest : MediatR.IRequest<Message>
//{
//    public IRequest Request { get; protected set; }
//    public void SetRequest<TRequest>(TRequest request) where TRequest : class, IRequest<Message>
//    { }
//    //=> Request = request;

//}

public record EditRequest<TEntity> : ResponseMessage<TEntity>  where TEntity : IEnumerable<IResponseChannel>
{
    public EditRequest(ChatId chatId, int messageId, string text) : base(text)
    {
    }

    public override ChatId ChatId { get; }
}
