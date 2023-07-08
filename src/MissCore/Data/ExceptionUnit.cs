using LinqToDB.Mapping;
using MissBot.Abstractions.Actions;
using MissCore.Bot;

namespace MissCore.Data
{
    [Table("##BotUnits")]
    public record ExceptionUnit : Unit<Exception>
    {
        [Column("Unit")]
        public override string UnitKey { get; set; }

        [Column("Entity")]
        public override string EntityKey { get; set; }
    }
}
