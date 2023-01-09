using Newtonsoft.Json;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace MissCore.Configuration
{
    /// <summary>
    /// Configurations for the bot
    /// </summary>
    public interface IBotConnectionOptions
    {
        /// <summary>
        /// Bot token id
        /// </summary>
        string Token { get; }

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
        UpdateType[] AllowedUpdates { get; }

        /// <summary>
        /// Limits the number of updates to be retrieved. Values between 1-100 are accepted.
        /// Defaults to 100 when is set to <c>null</c>.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the value doesn't satisfies constraints
        /// </exception>
        sbyte Limit { get; }

        /// <summary>
        /// Indicates if all pending <see cref="Update"/>s should be thrown out before start
        /// polling. If set to <c>true</c> <see cref="AllowedUpdates"/> should be set to not
        /// <c>null</c>, otherwise <see cref="AllowedUpdates"/> will effectively be set to
        /// receive all <see cref="Update"/>s.
        /// </summary>
        bool ThrowPendingUpdates { get; set; }

        string BaseFileUrl { get; }
        string BaseRequestUrl { get; }
        Func<Exception, CancellationToken, Task> ConnectionErrorHandler { get; }
        IExceptionParser ExceptionsParser { get; }
        bool LocalBotServer { get; }
        JsonSerializerSettings SerializeSettings { get; }
        bool UseTestEnvironment { get; }
    }
}
