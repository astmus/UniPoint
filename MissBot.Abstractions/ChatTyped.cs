using System;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types.Enums;
using TG = Telegram.Bot.Types.Chat;
/// <summary>
/// This object represents a message.
/// </summary>
namespace MissBot.Abstractions
{
    public class ChatTyped : TG
    {
        public IRequest<Message<TEntity>> GetResponse<TEntity>()
                => new ResponseRequest<TEntity>(Id);
        public IRequest<bool> SetAction(ChatAction action)
            => new SendChatActionRequest(Id, action);
    }
}
