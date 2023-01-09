using MissCore.Configuration;
using Newtonsoft.Json;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types.Enums;

namespace BotService.Configuration
{


    /// <summary>
    /// This class is used to provide configuration for <see cref="BotClient"/>
    /// </summary>

    internal class BotConnectionOptions : IBotConnectionOptions
    {
        /// <summary>
        /// BotConnectionOptions
        /// </summary>
        public BotConnectionOptions()
        {
            SerializeSettings = new JsonSerializerSettings();
            SerializeSettings.NullValueHandling = NullValueHandling.Ignore;
            SerializeSettings.DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate;
            SerializeSettings.ConstructorHandling = ConstructorHandling.AllowNonPublicDefaultConstructor;
            SerializeSettings.MissingMemberHandling = MissingMemberHandling.Ignore;
            SerializeSettings.PreserveReferencesHandling = PreserveReferencesHandling.Arrays;
            SerializeSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;

        }
        public string Token { get; internal set; }
        public TimeSpan Timeout { get; internal set; }

        /// <summary>
        ///  Setup custom parser of success response
        /// </summary>
        public bool UseCustomParser { get; internal set; }
        /// <summary>
        /// Used to change base url to your private bot api server URL. It looks like
        /// http://localhost:8081. Path, query and fragment will be omitted if present.
        /// </summary>
        public string BaseUrl { get; internal set; }

        /// <summary>
        /// Unique identifier for the bot from bot token. For example, for the bot token
        /// "1234567:4TT8bAc8GHUspu3ERYn-KGcvsvGB9u_n4ddy", the bot id is "1234567".
        /// Token format is not public API so this property is optional and may stop working
        /// in the future if Telegram changes it's token format.
        /// </summary>
        public long BotId { get; internal set; }
        /// <summary>
        /// Indicates that test environment will be used
        /// </summary>
        public bool UseTestEnvironment { get; internal set; }

        /// <summary>
        /// Indicates that local bot server will be used
        /// </summary>
        public bool LocalBotServer { get; internal set; }

        /// <summary>
        /// Contains base url for downloading files
        /// </summary>
        public string BaseFileUrl { get; internal set; }
        /// <summary>
        /// Contains base url for making requests
        /// </summary>
        public string BaseRequestUrl { get; internal set; }

        /// <summary>
        /// Serialization settings for parse response
        /// </summary>
        public JsonSerializerSettings SerializeSettings { get; internal set; }

        /// <summary>
        /// Custom parser of throwed exception, defalt is <see cref="DefaultExceptionParser"/>
        /// </summary>
        public IExceptionParser ExceptionsParser { get; internal set; } = new DefaultExceptionParser();

        #region receiveSettings
        public uint Offset { get; } = 0;
        public sbyte Limit { get; } = 10;
        public bool ThrowPendingUpdates { get; set; }
        public UpdateType[] AllowedUpdates { get; internal set; }
        #endregion

        /// <summary>
        /// Handler of connection exception
        /// </summary>
        public Func<Exception, CancellationToken, Task> ConnectionErrorHandler { get; internal set; }

    }
}
