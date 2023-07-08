namespace MissBot.Entities.Abstractions
{
	public interface IBotEntity
	{
		string EntityKey { get; }
		virtual string StringValue
			=> EntityKey;
	}
	public interface IUnitEntity
	{
		string UnitKey { get; }
	}
}
