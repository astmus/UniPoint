using MissBot.Abstractions.Actions;
using MissBot.Entities.Query;
using MissBot.Entities.Results.Inline;
using MissCore.Collections;

namespace MissBot.Response
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record InlineResultUnit : Unit<InlineQuery>
    {
        [JsonProperty]
        public InlineQueryResultType Type { get; set; } = InlineQueryResultType.Article;

        [JsonProperty]
        public InputMessageContent InputMessageContent
            => new InputTextMessageContent(Format());

        [JsonProperty]
        public string Id { get; set; }
        [JsonProperty]
        public string Title { get; set; }
        [JsonProperty]
        public string Description { get; set; }        
        /// <summary>
        /// Optional. Inline keyboard attached to the message
        /// </summary>
        [JsonProperty("reply_markup",DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IActionsSet Actions { get; set; }
    }
}
