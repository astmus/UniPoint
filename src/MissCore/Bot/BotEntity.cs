using LinqToDB.Mapping;
using MissBot.Entities.Abstractions;

namespace MissCore.Bot
{
	[Table("##BotUnits")]
    public record BotEntity<TEntity> :  IBotEntity
    {
        [Column("Unit")]
        public virtual string UnitKey { get; set; }
        static readonly string Key = typeof(TEntity).Name;
        //public static string EntityKey
        //    => Key;
        [Column("Entity")]
        public virtual string EntityKey => Key;        
    }

    [Table("##BotUnits")]
    public record EntityAction
    {
        [Column("Unit")]
        public virtual string Unit { get; set; }
        [Column("Entity")]
        public virtual string Action { get; set; }
    }
}
