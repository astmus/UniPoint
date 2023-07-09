using MissBot.Entities.Abstractions;
using MissBot.Entities.Query;

namespace MissBot.Abstractions.Bot
{
	public record InlineQuery<TOf> : InlineQuery, IBotEntity
    {
        [JsonIgnore]
        public string UnitKey { get; set; }
        [JsonIgnore]
        public string Entity { get; }
    }
}


