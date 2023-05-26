using System.ComponentModel.DataAnnotations.Schema;

namespace MissBot.Abstractions.Entities
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record BotCommand : Unit, IBotCommand
    {
        public virtual string Action { get; set; }

        [JsonProperty("description")]
        public virtual string Description { get; set; }

        [JsonIgnore]
        public override string Entity { get; set; } = nameof(BotCommand);

        [JsonProperty("command")]
        public  string Command => $"/{Action}";

        public virtual string Payload { get; set; }
        public virtual string Template { get; set; }
        public virtual string Unit { get; set; }
    }
}
