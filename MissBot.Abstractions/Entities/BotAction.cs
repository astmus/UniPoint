using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using Telegram.Bot.Types.ReplyMarkups;

namespace MissBot.Abstractions.Entities
{
    public record BotAction : BotCommand, IBotAction, IBotCommandInfo
    {
        static BotAction()
        {
            // Unit<TEntity>.MetaData
        }
        public static implicit operator BotAction(string data) =>
            new BotAction() { Command = data };
        public static implicit operator InlineKeyboardButton(BotAction s) =>
            InlineKeyboardButton.WithCallbackData(s.Command, string.Format(s.Placeholder, $"{s.Entity}.{s.Command}.{s.Id}"));
        [JsonProperty]
        public string Id { get; init; }
        [JsonProperty]
        public string Entity { get; set; }
        [JsonProperty]
        public string Placeholder { get; set; }
        [JsonProperty]
        public string Payload { get; set; }
        [JsonProperty]
        public override string Command { get; set; }
    }
}
