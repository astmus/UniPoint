using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissCore.Data
{
    public record BotUnitAction<TUnit> : BotUnit, IBotUnitAction<TUnit> where TUnit : class
    {
        public string Action { get; set; }
        public string Command { get; set; }
        public Id<TUnit> UnitIdentifier { get; set; }

        public static implicit operator BotUnitAction<TUnit>(string data)
            => JsonConvert.DeserializeObject<BotUnitAction<TUnit>>(data);
        public static implicit operator InlineKeyboardButton(BotUnitAction<TUnit> s) =>
            InlineKeyboardButton.WithCallbackData(s.Action ?? string.Format(s.Template, s.Entity, s.Action));
    }

    public record InlineEntityAction<TEntity> : BotUnitAction<TEntity>, IBotUnitAction<TEntity> where TEntity : class
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
