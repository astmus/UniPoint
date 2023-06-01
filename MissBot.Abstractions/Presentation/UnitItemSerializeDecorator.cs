using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.Presentation
{
    public interface IDecorator<in TComponent>
    {
        void SetComponent(TComponent item);
    }
    public interface ISerializeDecorator<in TUnit> : IDecorator<TUnit> where TUnit: IUnitItem
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

        public void SetComponent(IUnitItem item)
            => component = item;

        public abstract string Serialize();
    }
}
