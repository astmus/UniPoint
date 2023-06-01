using MissBot.Entities.Query;

namespace MissBot.Abstractions.Entities
{
    public record InlineQuery<TOf> : InlineQuery, IBotEntity
    {
        [JsonIgnore]
        public string Unit { get; set; }
        [JsonIgnore]
        public string Entity { get; }
    }
}


