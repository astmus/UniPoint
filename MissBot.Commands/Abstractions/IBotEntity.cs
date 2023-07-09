namespace MissBot.Entities.Abstractions
{
	public interface IBotEntity
	{
		string Entity { get; }
		virtual string StringValue
			=> Entity;
	}
	public interface IUnitEntity
	{
		string Unit
			=> nameof(IUnitEntity);
	}
}
