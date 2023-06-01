using MissBot.Abstractions.Presentation;

namespace MissBot.Abstractions.Configuration
{
    public interface IBotUnitBuilder
    {
        IBotUnitBuilder Apply<TDecorator>() where TDecorator:UnitItemSerializeDecorator;
    }
}
