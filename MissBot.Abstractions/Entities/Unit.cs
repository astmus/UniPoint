using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.Entities
{
    public abstract record Unit : IUnit, IBotEntity
    {
        public virtual string StringValue
            => Entity;
        public abstract string Entity { get; set; }
        public virtual IMetaData Meta { get; set; }
        public virtual void InitializaMetaData() { }
        public virtual string Format(IUnit.Formats format = IUnit.Formats.Table | IUnit.Formats.PropertyNames)
            => Entity;
    }
}
