using LinqToDB.Mapping;
using MissBot.Abstractions.Actions;
using MissBot.Entities.Abstractions;
using MissCore.Data;

namespace MissCore.Bot
{
	[Table("##BotUnits")]
    public record WithRequest : Unit, IUnitRequest, IExtendableUnit, IBotEntity
    {
        string Unit;
        string UnitCommand;

        public IEnumerable<IUnitParameter> Params { get; }
        public RequestOptions Options { get; set; }
        public string Template { get; set; }
        public string Extension { get; set; }
        [Column("Entity")]
        public string EntityKey { get; }

        public WithRequest()
        {
        }

        //public IUnitRequest OnUnit<TUnit>(TUnit unit) where TUnit : class, IUnitIdentible, IExtendableUnit
        //    => this with { Unit = unit.UnitKey, UnitCommand = unit.Extension };

        public override string ToString()
            => GetCommand();

        public string GetCommand()
            => string.Format(Template, Unit, UnitCommand);
    }
}
