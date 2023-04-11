namespace MissBot.Abstractions.Configuration
{
    public interface IBotOptionsBuilder
    {
        IBotOptionsBuilder ReceiveCallBacks();
        IBotOptionsBuilder ReceiveInlineQueries();
        IBotOptionsBuilder ReceiveInlineResult();
        IBotOptionsBuilder TrackMessgeChanges();
    }
}
