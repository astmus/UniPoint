
namespace MissCore.Configuration
{
    public interface IBotOptionsBuilder
    {
        IBotOptionsBuilder ReceiveCallBacks();
        IBotOptionsBuilder ReceiveInlineQueries();
        IBotOptionsBuilder ReceiveInlineResult();
        IBotOptionsBuilder TrackMessgeChanges();
        IBotOptionsBuilder UseCustomUpdateHandler();
        IBotOptionsBuilder SetTimeout(TimeSpan timeout);
        IBotOptionsBuilder SetToken(string token, string baseUrl = null, bool useTestEnvironment = false);
        IBotOptionsBuilder SetExceptionHandler(Func<Exception, CancellationToken, Task> handlerFactory);
        IBotConnectionOptions Build();
    }
}
