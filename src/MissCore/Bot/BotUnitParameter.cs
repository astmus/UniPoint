using LinqToDB.Mapping;
using MissBot.Abstractions.Actions;
using MissBot.Entities;
using Newtonsoft.Json.Linq;

namespace MissCore.Bot
{
    [Table("##UnitParameters")]
    public record BotUnitParameter : UnitParameterBase, IUnitParameter
    {
        [Column]
        public virtual string Unit { get; set; }

        [Column]
        public override string Name { get; set; }

        [Column]
        public virtual string Template { get; set; }

        [Column]
        public override object Value { get; set; }
    }

    [Table("##UnitParameters")]
    public record UnitParameter(string Name, object Value): IUnitParameter
    {        
    }

}
