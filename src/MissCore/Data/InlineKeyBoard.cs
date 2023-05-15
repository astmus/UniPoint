using MissBot.Abstractions.Actions;
using MissCore.Bot;
using MissCore.Collections;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissCore.Data
{
    public record EntityAction<TEntity> : BotUnit2<TEntity>, IBotAction<TEntity> where TEntity : class
    {
        public string CommandAction { get; }

        public static implicit operator EntityAction<TEntity>(string data)
            => JsonConvert.DeserializeObject<EntityAction<TEntity>>(data);
        public static implicit operator InlineKeyboardButton(EntityAction<TEntity> s) =>
            InlineKeyboardButton.WithCallbackData(s.Command ?? string.Format(s.Placeholder, s.Entity, s.Command, s.Unit));


    }



    public record InlineEntityAction<TEntity> : EntityAction<TEntity>, IBotAction<TEntity> where TEntity : Unit
    {
        public string Text { get; set; }
        public virtual string Action { get; init; }
        public virtual string Data
            => string.Format(Action, Text);
        public static implicit operator InlineEntityAction<TEntity>(string data) =>
            new InlineEntityAction<TEntity>() { Action = data };
        public static implicit operator InlineKeyboardButton(InlineEntityAction<TEntity> s) =>
            InlineKeyboardButton.WithCallbackData(s.Text ?? s.Action[2..], $"{s.Action}.{s.Text}");
    }


    public class InlineKeyBoard : List<InlineKeyboardButton>
    {
        public InlineKeyboardMarkup GetKeyboard
            => new InlineKeyboardMarkup(this);

        public InlineKeyBoard Append(BotAction action)
        {
            // Add(action);
            return this;
        }
    }
}
