namespace MissBot.Entities.Abstractions;


public interface IUnitAction : IBotAction
{
	IIdentibleUnit UnitContext { get; set; }
}

public interface IUnitAction<out TUnit> : IBotAction<TUnit>, IUnitAction
{

}

