using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.Entities
{
    public abstract record BaseUnit : IUnit
    {
        public virtual object Identifier { get; }
        [JsonIgnore]
        public virtual string StringValue
            => Entity;
        public abstract string Entity { get; set; }
        [JsonIgnore]
        public virtual IMetaData Meta { get; set; }
        
        public abstract void InitializeMetaData();
        public virtual string Format(IUnit.Formats? format = default)
            => Entity;
        public virtual IMetaItem GetItem(int index)
            => Meta.GetItem(index);
    }
}
