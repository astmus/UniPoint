using MissBot.Identity;

namespace MissBot.Entities.Abstractions
{
	public interface IBotAction : IBotEntity
	{
		string Action { get; }
		string Unit { get; }
	}

	public interface IBotAction<TUnit> : IBotAction, IIdentibleUnit, IExtendableUnit
	{
		Id<TUnit> Id { get; set; }
	}
}
