using MissBot.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

namespace MissBot.Entities.Results.Inline
{
    public interface IInlineContent
    {
        bool? DisableWebPagePreview { get; set; }
        MessageEntity[] Entities { get; set; }
        ParseMode? ParseMode { get; set; }
        string Value { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record InlineContent<T> : IInlineContent
    {
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool? DisableWebPagePreview { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public MessageEntity[] Entities { get; set; }
        [JsonProperty("message_text")]
        public string Value { get; set; }
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode? ParseMode { get; set; } = Telegram.Bot.Types.Enums.ParseMode.Html;
    }
}
