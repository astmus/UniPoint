using LinqToDB.Mapping;
using MissBot.Abstractions;

namespace MissCore.Bot
{
    [Table("##BotUnits")]
    public record BotEntity<TEntity> :  IBotEntity
    {
        [Column()]
        public virtual string Unit { get; set; }
        static readonly string Key = typeof(TEntity).Name;
        public static string EntityKey
            => Key;

        public virtual string Entity => Key;

        public string ToString(string? format, IFormatProvider? formatProvider)
        {
            throw new NotImplementedException();
        }
    }

    [Table("##BotUnits")]
    public record EntityAction
    {
        [Column()]
        public virtual string Unit { get; set; }
        [Column("Entity")]
        public virtual string Action { get; set; }
    }
}
