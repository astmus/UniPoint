using Newtonsoft.Json.Linq;

namespace MissBot.Abstractions.Entities
{
    public abstract record Unit : IUnit, IBotEntity
    {
        public virtual string StringValue
            => Format();
        public abstract string Entity { get; }
        public virtual IMetaData Meta { get; set; }
        public virtual void InitializaMetaData() { }
        public virtual string Format(IUnit.Formats format = IUnit.Formats.Line | IUnit.Formats.PropertyNames)
            => ToString();
    }
}
