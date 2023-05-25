using LinqToDB.Mapping;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;

namespace MissCore.Bot
{
    [Table("##BotUnits")]
    public record BotUnitCommand : BotCommand, IBotUnitCommand, IBotDataEntity
    {
        [Column("Unit")]
        public override string Unit { get; set; }
        public override string Entity
            => base.Entity;

        [Column("Entity")]
        public override string Action { get => base.Action; set => base.Action = value; }

        [Column("Description")]
        public override string Description { get => base.Description; set => base.Description = value; }

        [Column("Payload")]
        [JsonProperty]
        public override string Payload { get => base.Payload; set => base.Payload = value; }
    }
}
