using MissBot.Abstractions.Presentation;

namespace MissCore.Presentation.Decorators
{
    public record BoldNameUnitItemDecorator : UnitItemSerializeDecorator
    {
        public override string Name
            => $"<b>{component.Name}:</b>";
        public override object Value
            => component.Value;

        public override string Serialize()
            => $"{Name} {Value}";
    }
}
