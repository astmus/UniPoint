using System.ComponentModel.DataAnnotations.Schema;

namespace MissBot.Abstractions.Entities
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record BotCommand : BaseUnit, IBotCommand
    {
        public override string UnitKey { get; set; } = nameof(BotCommand);
        [JsonIgnore]
        public override object Identifier
            => $"{UnitKey}.{Action}";
        [JsonProperty("command")]
        public  string Command => $"/{Action.ToLowerInvariant()}";
        [JsonProperty("description")]
        public virtual string Description { get; set; }
        //public override string EntityKey { get; set; } = Action;
        public virtual string Action { get; set; }
        public virtual string Payload { get; set; }
        public virtual string Template { get; set; }

        //public virtual string UnitKey { get; set; }

        public override void InitializeMetaData()
        {            
        }
    }
}
