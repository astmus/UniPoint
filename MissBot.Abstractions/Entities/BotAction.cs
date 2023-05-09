using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissBot.Abstractions.Entities
{
    public record BotAction<TEntity> : BotEntity, IBotAction<TEntity>
    {
        public static readonly string UnitName = typeof(TEntity).Name;

        public static implicit operator BotAction<TEntity>(string data) =>
            new BotAction<TEntity>() /*{ Command = data }*/;
        public static implicit operator InlineKeyboardButton(BotAction<TEntity> s) =>
            InlineKeyboardButton.WithCallbackData(s.CommandAction, string.Format(s.Placeholder, $"{s.Entity}.{s.CommandAction}.{s.Id}"));
        [JsonProperty]
        public string Id { get; init; }
        [JsonProperty]
        public override string Entity
            => UnitName;
        [JsonProperty]
        public string CommandAction
            { get;  }
        public string Placeholder { get; set; }
        public string Payload { get; set; }
        public string[] Params { get; set; }
        public string Command { get; set; }
    }
}
