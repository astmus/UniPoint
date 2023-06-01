using MissBot.Abstractions.Presentation;

namespace MissCore.Presentation.Decorators
{
    public record BoldNameUnitItemDecorator : UnitItemSerializeDecorator
    {
        public override string ItemName
            => $"<b>{component.ItemName}:</b>";
        public override object ItemValue
            => component.ItemValue;

        public override string Serialize()
            => $"{ItemName} {ItemValue}";
    }
}
