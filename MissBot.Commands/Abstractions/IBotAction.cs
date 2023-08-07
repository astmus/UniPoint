using MissBot.Identity;

namespace MissBot.Entities.Abstractions
{
	public interface IBotAction : IUnitEntity
	{
		string Action { get; }
	}

	public interface IBotAction<in TUnit> : IBotAction, IIdentibleUnit, ITemplatedUnit
	{
		Id Id { get; set; }
	}
}
