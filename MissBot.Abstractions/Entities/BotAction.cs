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
        public string Id { get; init; }
        public string Entity { get; set; }
        public string Placeholder { get; set; }
        public string Payload { get; set; }
        public override string Command { get; set; }
    }
}
