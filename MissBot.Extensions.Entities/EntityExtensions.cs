using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

namespace MissBot.Commands
{
    public static class EntityExtensions
    {
        //public static IRequest<Message<TEntity>> Response<TEntity>(this Message message, string text) where TEntity : class
        //        => new SendResponse<TEntity>(message.Chat.Id, text) { };
        //public static IRequest<Message<TEntity>> Response<TEntity>(this Telegram.Bot.Types.Chat chat, TEntity text) where TEntity : class
        //=> new SendResponse<TEntity>(chat.Id, text.ToString()) { };
        public static IRequest<TMessage> Update<TMessage>(this TMessage message) where TMessage:Message
            => new EditMessageRequest<TMessage>(message.Chat.Id, message.MessageId, message.Text) { };
        public static IRequest<bool> SetAction(this Telegram.Bot.Types.Chat chat, ChatAction action)
            => new SendChatActionRequest(chat.Id,action);
    }
}
