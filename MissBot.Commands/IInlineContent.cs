using Telegram.Bot.Types.Enums;

namespace MissBot.Entities
{
    public interface IInlineContent
    {
        bool? DisableWebPagePreview { get; set; }
        MessageEntity[] Entities { get; set; }
        string MessageText { get; }
        ParseMode? ParseMode { get; set; }
    }
}