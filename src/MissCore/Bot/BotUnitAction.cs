using System.Collections.Generic;
using LinqToDB.Mapping;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Entities;
using MissBot.Common.Extensions;

namespace MissCore.Bot
{
    [Table("##BotUnits")]
    public record BotUnitAction : BotUnit, IBotUnitAction, IBotDataEntity, IBotEntity
    {
        [Column]
        public override string UnitKey { get; set; }

        [Column]
        public override string EntityKey
            => Action;

        [Column("Entity")]
        public virtual string Action { get; set ; }

        [Column]       
        public override string Payload { get; set; }
    }

    [Table("##BotUnits")]
    public record BotUnitCommand : BotCommand, IBotUnitAction, IBotDataEntity, IBotEntity, IBotUnit
    {
        [Column("Unit")]
        public override string UnitKey { get; set; }

        [Column("Entity")]
        public override string EntityKey
            => Action;

        [Column("Entity")]
        public override string Action { get; set; }

        [Column]
        public override string Description { get; set; }

        [Column]
        public override string Payload { get ; set ; }

        [Column()]
        public virtual string Parameters { get; set; }

       
    }
}
