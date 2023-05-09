using Newtonsoft.Json;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace MissBot.Abstractions.Configuration
{
    /// <summary>
    /// Configurations for the bot
    /// </summary>
    public interface IConnectionOptions
    {


        /// <summary>
        /// Waiting time before create new request
        /// </summary>
        TimeSpan Timeout { get; }

        /// <summary>
        /// Identifier of the first update to be returned. Will be ignored if
        /// <see cref="ThrowPendingUpdates"/> is set to <c>true</c>.
        /// </summary>
        uint Offset { get; }
        /// <summary>
        /// Indicates which <see cref="UpdateType"/>s are allowed to be received.
        /// In case of <c>null</c> the previous setting will be used
        /// </summary>


        string BaseConnectionAddress { get; }
        Func<Exception, CancellationToken, Task> ConnectionErrorHandler { get; }
        IExceptionParser ExceptionsParser { get; }

        JsonSerializerSettings SerializeSettings { get; }

    }
}
