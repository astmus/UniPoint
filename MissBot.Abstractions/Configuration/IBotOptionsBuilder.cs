using MissBot.Abstractions.Bot;

namespace MissBot.Abstractions.Configuration
{
    public interface IBotOptionsBuilder
    {
        IBotOptionsBuilder ReceiveCallBacks();
        IBotOptionsBuilder ReceiveInlineQueries();
        IBotOptionsBuilder ReceiveInlineResult();
        IBotOptionsBuilder TrackMessgeChanges();
        IBotUnitBuilder AddResponseUnit<TUnit>() where TUnit: BaseBotUnit;
    }
}
