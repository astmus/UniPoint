using MissBot.Abstractions.Actions;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.Entities
{
    [JsonObject(MemberSerialization = MemberSerialization.OptIn, ItemNullValueHandling = NullValueHandling.Ignore)]
    public abstract record BaseUnit : IUnit, IBotEntity
    {
        [JsonIgnore]
        public virtual object Identifier
            => $"{UnitKey}.{EntityKey}";

        [JsonProperty("Unit", Order = int.MinValue)]
        public abstract string UnitKey { get; set; }

       [JsonProperty("Entity", Order = int.MinValue + 1)]
        public virtual string EntityKey { get; set; }
        public virtual IMetaData Meta { get; set; }
        
        public virtual IActionsSet Actions { get; set; }

        public abstract void InitializeMetaData();

        public virtual IUnitItem GetItem(int index)
            => Meta.GetItem(index);
    }
}
