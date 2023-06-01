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
    [JsonObject]
    public record Unit : BaseUnit, IUnit
    {
        public override IActionsSet Actions { get; set; }
        public override string Entity { get; set; }
        public override void InitializeMetaData()
            => Meta ??= MetaData.Parse(this);
    }
    /// <summary>
    /// [DebuggerDisplay($"Value: {nameof(Text)}")]
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="Entity"></param>
    [JsonObject]
    [Table("##BotUnits")]
    public record Unit<TEntity> : Unit, IUnit<TEntity>
    {
        public static readonly string Key = typeof(TEntity).Name;
        public static readonly Id<TEntity> Id = Id<TEntity>.Value;

        [JsonProperty("Unit", Order = int.MinValue)]
        public TEntity UnitValue { get; set; }

        [JsonIgnore]
        public override string StringValue
            => Meta?.StringValue ?? Key;

        [Column()]
        [JsonIgnore]
        public override string Entity { get; set; } = Key;
        [JsonIgnore]
        public override IMetaData Meta { get; set; }

        public static Unit<TEntity> Parse<TData>(TData data)
        {
            var unit = new Unit<TEntity>();
            unit.Meta = MetaData.Parse(data);
            return unit;
        }

        public class Collection : List<TEntity> { }
    }
}
