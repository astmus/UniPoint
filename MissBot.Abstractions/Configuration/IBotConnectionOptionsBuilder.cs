using Newtonsoft.Json;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace MissCore.Configuration
{
    /// <summary>
    /// Configurations for the bot
    /// </summary>
    public interface IBotConnectionOptionsBuilder : IBotOptionsBuilder
    {
        IBotConnectionOptionsBuilder SetTimeout(TimeSpan timeout);
        IBotConnectionOptionsBuilder SetExceptionHandler(Func<Exception, CancellationToken, Task> handlerFactory);
        IBotConnectionOptionsBuilder UseCustomUpdateHandler();
        IBotConnectionOptionsBuilder SetToken(string token, string baseUrl = default, bool useTestEnvironment = false);
        IBotConnectionOptions Build();
    }
}
