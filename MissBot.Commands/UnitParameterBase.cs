namespace MissBot.Entities
{
	public abstract record UnitParameterBase
	{
		public virtual string Name { get; set; }
		public virtual object Value { get; set; }
	}

	public record UnitRequestParameter<TName, TValue>(TName Name, TValue Value)
	{
	}
}
