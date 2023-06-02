using MissBot.Entities.Query;

namespace MissBot.Abstractions.Entities
{
    public record InlineQuery<TOf> : InlineQuery, IBotEntity
    {
        [JsonIgnore]
        public string UnitKey { get; set; }
        [JsonIgnore]
        public string EntityKey { get; }
    }
}


