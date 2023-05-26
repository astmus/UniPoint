using System.Runtime.CompilerServices;
using System.Text.Json.Nodes;
using LinqToDB.Mapping;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Abstractions.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MissCore.Collections
{

    /// <summary>
    /// [DebuggerDisplay($"Value: {nameof(Text)}")]
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    /// <param name="Entity"></param>
    [JsonObject]
    public record Unit<TEntity> : Unit, IUnit<TEntity>
    {        
        public static readonly string Key = typeof(TEntity).Name;
        public static readonly Id<TEntity> Id = Id<TEntity>.Value;

        public string this[string path]
            => Meta.GetValue(path).ToString();

        [JsonIgnore]
        public override string StringValue
            => Meta?.StringValue ?? Key;

        [Column()]
        [JsonIgnore]
        public override string Entity { get; set; } = Key;
        [JsonIgnore]
        public override IMetaData Meta { get; set; }

        public override void InitializaMetaData()
            => Meta ??= MetaData<TEntity>.Parse(this);
        public static DataMap Parse(TEntity entity)
            => DataMap.Parse(entity);

        public override string Format(IUnit.Formats format = IUnit.Formats.Table | IUnit.Formats.PropertyNames)
        {
            if (Meta != null)
                return GetFormat(format);

            return string.Empty;
        }

        private string GetFormat(IUnit.Formats format) => format switch
        {
            IUnit.Formats.UnitName => Entity + '\n',
            IUnit.Formats.Line => string.Join(" ", Meta.Keys.Select(key => Meta.GetValue(key)))+'\n',
            IUnit.Formats.Table => string.Join('\n', Meta.Keys.Select(key => $"{Meta.GetValue(key)}\n")),
            IUnit.Formats.Line | IUnit.Formats.PropertyNames => Meta.StringValue+'\n',
            IUnit.Formats.Table | IUnit.Formats.PropertyNames => string.Join('\n', Meta.Keys.Select(key => $"<b>{key}:</b> {Meta.GetValue(key)}"))+'\n',
            _ => null
        };

        public class Collection : List<TEntity> { }
    }
}
