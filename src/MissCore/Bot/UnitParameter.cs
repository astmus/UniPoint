using LinqToDB.Mapping;
using MissBot.Entities;
using Newtonsoft.Json.Linq;

namespace MissCore.Bot
{
    [Table("##UnitParameters")]
    public record UnitParameter : BotUnitParameter
    {
        [Column]
        public override string Unit { get; set; }
        [Column]
        public override string Name { get; set; }
        [Column]
        public override string Template { get; set; }
        [Column]
        public override string Value { get; set; }
    }
    
}
