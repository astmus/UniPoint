using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissCore.Entities
{
    public record InlineEntityAction : ValueUnit, IEntityAction
    {
        public string Text { get; set; }
        public object? Id { get; set; }
        public virtual string Action { get; init; }
     
        public static implicit operator InlineEntityAction(string data) =>
            new InlineEntityAction() { Action = data };
        public static implicit operator InlineKeyboardButton(InlineEntityAction s) =>
            InlineKeyboardButton.WithCallbackData(s.Text ?? s.Action[2..], $"{s.Action}.{s.Id}");
    }

    public record EntityAction<TEntity> : EntityAction
    { }

    public record EntityAction : IBotCommand
    {
        public string Payload { get; set; }
        public string[] Params { get; set; }
        public string Command { get; }
        public string Description { get; }
        public string WithCondition(params object[] param)
            => string.Format(Payload, param);
    }

    public record InlineEntityAction<TEntity> : EntityAction<TEntity>, IEntityAction<TEntity>
    {
        public string Text { get; set; }
        public object? Id { get; set; }
        public virtual string Action { get; init; }
        public virtual string Data
            => string.Format(Action, Id);
        public static implicit operator InlineEntityAction<TEntity>(string data) =>
            new InlineEntityAction<TEntity>() { Action = data };
        public static implicit operator InlineKeyboardButton(InlineEntityAction<TEntity> s) =>
            InlineKeyboardButton.WithCallbackData(s.Text ?? s.Action[2..], $"{s.Action}.{s.Id}");
    }


    public class InlineKeyBoard : List<InlineKeyboardButton>
    {        
        public InlineKeyboardMarkup GetKeyboard
            => new InlineKeyboardMarkup(this);               

        public InlineKeyBoard Append(InlineEntityAction action)
        {
            Add(action);
            return this;
        }        
    }
}
