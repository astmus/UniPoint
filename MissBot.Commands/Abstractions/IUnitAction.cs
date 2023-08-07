namespace MissBot.Entities.Abstractions;


public interface IUnitAction : IBotAction
{
	IIdentibleUnit UnitContext { get; set; }
}

public interface IUnitAction<in TUnit> : IBotAction<TUnit>, IUnitAction
{
	void SetUnitContext<TCUnit>(TCUnit unit) where TCUnit : class, TUnit;
}

