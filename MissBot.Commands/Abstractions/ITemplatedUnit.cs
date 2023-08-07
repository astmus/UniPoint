namespace MissBot.Entities.Abstractions
{
	public interface ITemplatedUnit
	{
		string Template { get; set; }
		string Format { get; }
	}
}
