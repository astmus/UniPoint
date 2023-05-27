using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore.Bot;
using MissCore.Collections;
using MissCore.Data.Entities;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissCore.Data
{
    public record UnitAction<TUnit> : BotUnit, IBotUnitAction<TUnit> where TUnit : class
    {
        public string Action { get; set; }
        public string Command { get; set; }
        public Id<TUnit> Identifier { get; set; }

        public static implicit operator UnitAction<TUnit>(string data)
            => JsonConvert.DeserializeObject<UnitAction<TUnit>>(data);
        public static implicit operator InlineKeyboardButton(UnitAction<TUnit> s) =>
            InlineKeyboardButton.WithCallbackData(s.Action ?? string.Format(s.Template, s.Entity, s.Action));
    }

    public record InlineEntityAction<TEntity> : UnitAction<TEntity>, IBotUnitAction<TEntity> where TEntity : class
    {
        public string Text { get; set; }        
        public virtual string Data
            => string.Format(Action, Text);
        public static implicit operator InlineEntityAction<TEntity>(string data) =>
            new InlineEntityAction<TEntity>() { Action = data };
        public static implicit operator InlineKeyboardButton(InlineEntityAction<TEntity> s) =>
            InlineKeyboardButton.WithCallbackData(s.Text ?? s.Action[2..], $"{s.Action}.{s.Text}");
    }    
}
