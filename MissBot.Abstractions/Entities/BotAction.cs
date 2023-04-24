using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissBot.Abstractions.Entities
{
    public record BotAction<TUnit> : BotEntity, IBotAction<TUnit>
    {
        public static readonly string UnitName = typeof(TUnit).Name;

        public static implicit operator BotAction<TUnit>(string data) =>
            new BotAction<TUnit>() /*{ Command = data }*/;
        public static implicit operator InlineKeyboardButton(BotAction<TUnit> s) =>
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
