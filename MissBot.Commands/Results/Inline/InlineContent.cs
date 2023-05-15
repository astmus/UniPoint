using MissBot.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

namespace MissBot.Entities.Results.Inline
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record InlineContent
    {
        [JsonProperty]
        public bool? DisableWebPagePreview { get; set; }
        [JsonProperty]
        public MessageEntity[] Entities { get; set; }
        [JsonProperty]
        public string MessageText { get; set; }
        [JsonProperty]
        public ParseMode? ParseMode { get; set; }
    }
}
