using System.Collections;
using MissBot.Abstractions.Actions;
using MissBot.Entities.Abstractions;
using Newtonsoft.Json;

namespace MissBot.Abstractions.Bot
{
	[JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract record BaseBotUnit : BaseItem, IBotEntity
    {
        public override object Identifier
            => string.Join('.', UnitKey, EntityKey);

        [JsonProperty("Unit")]
        public abstract string UnitKey { get; set; }

        [JsonProperty("Entity", Order = int.MinValue + 1)]
        public virtual string EntityKey { get; set; }
    }

    public abstract record BaseUnit : BaseItem, IUnit
    {
        [JsonProperty("Unit", Order = int.MinValue)]
        public virtual string UnitKey { get; set; }

        [JsonIgnore]
        public abstract IEnumerator UnitEntities { get; }
        public abstract void SetContext(object data);

        //public IMetaData MetaData { get; set; }

        //public virtual string EntityKey { get; set; }
        //protected abstract T Get<T>([CallerMemberName] string name = null);
        //protected abstract void Set<T>(T item, [CallerMemberName] string name = null);
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract record BaseItem : IIdentibleUnit
    {
        public abstract object Identifier { get; }
    }

    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract record BaseBotAction : BaseItem, IBotAction
    {
        public override object Identifier
            => string.Join('.', Unit, Action);

        [JsonProperty]
        public virtual string Unit { get; set; }

        [JsonProperty("text", Required = Required.Always, Order = int.MinValue)]
        public virtual string Action { get; set; }

        public virtual string EntityKey
            => Action;
    }
}
