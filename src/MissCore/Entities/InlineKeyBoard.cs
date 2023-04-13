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
    public record InlineAction : ValueUnit, IEntityAction
    {
        public string Text { get; set; }
        public object? Id { get; set; }
        public virtual string Action { get; init; }
     
        public static implicit operator InlineAction(string data) =>
            new InlineAction() { Action = data };
        public static implicit operator InlineKeyboardButton(InlineAction s) =>
            InlineKeyboardButton.WithCallbackData(s.Text ?? s.Action[2..], $"{s.Action}.{s.Id}");
    }
       

    public record InlineAction<TEntity> : InlineAction, IEntityAction<TEntity>
    {
    
    }


    public class InlineKeyBoard : List<InlineKeyboardButton>
    {        
        public InlineKeyboardMarkup GetKeyboard
            => new InlineKeyboardMarkup(this);               

        public InlineKeyBoard Append(InlineAction action)
        {
            Add(action);
            return this;
        }        
    }
}
