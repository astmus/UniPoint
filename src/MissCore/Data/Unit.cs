
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Nodes;
using LinqToDB.Mapping;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using MissCore.Data.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissCore.Data
{
    
    [Table("##BotUnits")]    
    public record Unit : BaseUnit, IUnit
    {        
        [Column("Unit")]
        public override string UnitKey { get; set; }
        
        [Column("Entity")]
        public override string EntityKey { get; set; }

        [JsonIgnore]
        public override IActionsSet Actions { get; set; }

        public override void InitializeMetaData()
            => Meta ??= MetaData.Parse(this);
    }
    /// <summary>
    /// [DebuggerDisplay($"Value: {nameof(Text)}")]
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="Entity"></param>
    
    [Table("##BotUnits")]
    [JsonObject(MemberSerialization = MemberSerialization.OptOut, ItemNullValueHandling = NullValueHandling.Ignore)]
    public record Unit<TEntity> : Unit, IUnit<TEntity>
    {
        public static readonly string Key = typeof(TEntity).Name;
        public static readonly Id<TEntity> Id = Id<TEntity>.Value;
        
        public override string UnitKey { get; set; }

        [Column("Entity")]        
        public override string EntityKey { get; set; } = Key;

        [JsonProperty("Item", Order = int.MinValue+1)]
        public virtual TEntity Entity { get; set; }

        [JsonIgnore]
        public virtual string StringValue
            => Meta?.StringValue ?? Key;

        [JsonIgnore]
        public override IMetaData Meta { get; set; }

        public static Unit<TEntity> Parse<TData>(TData data)
        {
            var unit = Activator.CreateInstance<Unit<TEntity>>();
            unit.Meta = MetaData.Parse(data);
            return unit;
        }

        public class Collection : List<TEntity> { }
    }
}
