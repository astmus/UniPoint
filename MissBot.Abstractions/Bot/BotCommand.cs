using System.ComponentModel.DataAnnotations.Schema;
using MissBot.Entities.Abstractions;

namespace MissBot.Abstractions.Bot
{
	[JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    public record BotCommand : BaseBotAction, IBotCommand, IExtendableUnit
    {
        [Column]
        public override string Unit { get => base.Unit; set => base.Unit = value; }

        [Column("Entity")]
        public override string Action { get; set; }

        [JsonProperty("command")]
        public string Command => $"/{Action?.ToLowerInvariant()}";

        [JsonProperty("description")]
        public virtual string Description { get; set; }
        public virtual string Template { get; set; }
        public virtual string Extension { get; set; }
    }
}
