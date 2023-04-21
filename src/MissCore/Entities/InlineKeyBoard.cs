using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using Newtonsoft.Json;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissCore.Entities
{
    public record EntityAction<TEntity> : BotAction, IEntityAction<TEntity> where TEntity : class
    {
        public static implicit operator EntityAction<TEntity>(string data)
            => JsonConvert.DeserializeObject< EntityAction<TEntity>>(data );
        public static implicit operator InlineKeyboardButton(EntityAction<TEntity> s) =>
            InlineKeyboardButton.WithCallbackData(s.Command ?? string.Format(s.Placeholder, s.Entity, s.Command, s.Id));
    }

    public record BotEntityAction : IBotCommand
    {
        public string Payload { get; set; }
        public string[] Params { get; set; }
        public string Description { get; }
        public string Command { get; set; }

        public string WithCondition(params object[] param)
            => string.Format(Payload, param);
    }

    public record InlineEntityAction<TEntity> : EntityAction<TEntity>, IEntityAction<TEntity> where TEntity:ValueUnit
    {
        public string Text { get; set; }
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

        public InlineKeyBoard Append(BotAction action)
        {
            Add(action);
            return this;
        }        
    }
}
