using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;

namespace MissCore.Response
{
    public record InlineQuery<TOf> : InlineQuery, IBotEntity
    {        
        [JsonIgnore]
        public string Unit { get; set; }
        [JsonIgnore]
        public string Entity { get; }        
    }
}


