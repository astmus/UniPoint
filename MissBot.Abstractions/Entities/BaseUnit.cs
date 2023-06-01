using MissBot.Abstractions.Actions;
using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.Entities
{
    public abstract record BaseUnit : IUnit, IBotEntity
    {
        [JsonProperty(Order = 1)]
        public virtual string Unit { get; set; }
        [JsonIgnore]
        public virtual object Identifier { get; }
        [JsonIgnore]
        public virtual string StringValue
            => Entity;

        public abstract string Entity { get; set; }
        [JsonIgnore]
        public virtual IMetaData Meta { get; set; }
        [JsonIgnore]
        public virtual IActionsSet Actions { get; set; }
        [JsonIgnore]
        public virtual IUnit.DisplayFormat DisplayAs { get; set; } = IUnit.DisplayFormat.Table;

        public abstract void InitializeMetaData();

        public virtual IMetaItem GetItem(int index)
            => Meta.GetItem(index);
    }
}
