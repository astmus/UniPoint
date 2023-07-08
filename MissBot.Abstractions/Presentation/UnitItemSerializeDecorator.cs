using MissBot.Abstractions.Bot;

namespace MissBot.Abstractions.Presentation
{
    public interface IUnitDecorator<in TComponent>
    {
        IUnitItem SetComponent(TComponent item);
    }
    public interface ISerializeDecorator<in TUnit> : IUnitDecorator<TUnit> where TUnit: IUnitItem
    {
        string Serialize();
    }
    public interface IUnitItemDecorator :  ISerializeDecorator<IUnitItem>
    {
        string ItemName { get; }
        object ItemValue { get; }
    }

    public abstract record UnitItemSerializeDecorator : IUnitItem, IUnitItemDecorator
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
