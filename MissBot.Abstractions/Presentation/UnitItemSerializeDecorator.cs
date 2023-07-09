using MissBot.Abstractions.Bot;

namespace MissBot.Abstractions.Presentation
{
	public interface IUnitDecorator<in TUnit> where TUnit : IUnitItem
	{
		IUnitItem SetComponent(TUnit item);
	}
	public interface ISerializeDecorator<in TUnit> : IUnitDecorator<TUnit>, ISerializableUnit where TUnit : IUnitItem
	{

	}
	public interface IUnitItemDecorator : ISerializeDecorator<IUnitItem>, IUnitItem
	{
	}

	public abstract record UnitItemSerializeDecorator : IUnitItemDecorator
	{
		protected IUnitItem component { get; set; }

		public abstract string ItemName { get; }
		public abstract object ItemValue { get; }

		public IUnitItem SetComponent(IUnitItem item)
		{
			component = item;
			return this;
		}

		public abstract string Serialize();
	}
}
