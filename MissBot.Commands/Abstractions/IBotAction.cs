using MissBot.Identity;

namespace MissBot.Entities.Abstractions
{
	public interface IBotAction : IBotEntity
	{
		string Action { get; }
		string Unit { get; }
	}

	public interface IBotAction<out TUnit> : IBotAction, IIdentibleUnit, ITemplatedUnit
	{
		Id Id { get; set; }
	}
}
