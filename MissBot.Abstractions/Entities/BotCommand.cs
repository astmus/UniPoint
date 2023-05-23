using System.ComponentModel.DataAnnotations.Schema;

namespace MissBot.Abstractions.Entities
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record BotCommand : Unit, IBotCommand
    {
        /// <summary>
        /// Text of the command, 1-32 characters. Can contain only lowercase English letters, digits and underscores.
        /// </summary>  
        public virtual string Action { get; set; }

        /// <summary>
        /// Description of the command, 3-256 characters.
        /// </summary>
        [JsonProperty("description")]
        public virtual string Description { get; set; }
        [JsonIgnore]
        public override string Entity
            => nameof(BotCommand);

        [JsonProperty("command")]
        public  string Command
            => $"/{Action}";

        public virtual string Payload { get; set; }
        public virtual string Template { get; set; }
        public virtual string Unit { get; set; }
    }
}
