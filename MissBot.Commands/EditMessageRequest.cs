using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;


namespace MissBot.Commands;

public class MessageRequest : MediatR.IRequest<Message>
{
    public IRequest Request { get; protected set; }
    public void SetRequest<TRequest>(TRequest request) where TRequest : IRequest<Message>
        => Request = request;

}

public record EditRequest<TMessage> : EditMessageRequest<TMessage>, MediatR.IRequest<TMessage> where TMessage : Message
{
    public EditRequest(ChatId chatId, int messageId, string text) : base(chatId, messageId, text)
    {
    }
}
