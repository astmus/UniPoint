using LinqToDB.Mapping;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;

namespace MissCore.Bot
{
    [Table("##BotUnits")]
    public record BotUnitAction : BotUnit, IBotUnitAction, IBotDataEntity, IBotEntity
    {
        [Column]
        public override string Unit { get; set; }
        [Column]
        public override string Entity
            => Action;

        [Column("Entity")]
        public virtual string Action { get; set ; }

        //[Column("Description")]
        //public override string Description { get => base.Description; set => base.Description = value; }

        [Column("Payload")]
       
        public override string Payload { get => base.Payload; set => base.Payload = value; }
    }

    [Table("##BotUnits")]
    public record BotUnitCommand : BotCommand, IBotUnitAction, IBotDataEntity, IBotEntity, IBotUnit
    {
        [Column]
        public override string Unit { get; set; }
        [Column]
        public override string Entity
            => Action;

        [Column("Entity")]
        public override string Action { get; set; }

        [Column]
        public override string Description { get => base.Description; set => base.Description = value; }

        [Column]
        public override string Payload { get => base.Payload; set => base.Payload = value; }

        [Column()]
        public virtual string Parameters { get; set; }
    }
}
