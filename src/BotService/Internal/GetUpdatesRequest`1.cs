using MissCore.Abstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Telegram.Bot.Types.Enums;

namespace BotService.Internal
{
    [JsonObject(NamingStrategyType = typeof(SnakeCaseNamingStrategy))]
    internal record GetUpdatesRequest<T> : BaseRequest<T[]> where T : class, IUpdateInfo
    {
        /// <summary>
        /// Identifier of the first update to be returned
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public uint Offset { get; set; }

        /// <summary>
        /// Limits the number of updates to be retrieved. Values between 1—100 are accepted. Defaults to 100.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int Limit { get; set; }

        /// <summary>
        /// Timeout in seconds for long polling. Defaults to 0, i.e. usual short polling.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public uint Timeout { get; set; }

        /// <summary>
        /// List the types of updates you want your bot to receive. Specify an empty list to receive all updates regardless of type (default). If not specified, the previous setting will be used.
        /// </summary>
        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IEnumerable<UpdateType> AllowedUpdates { get; set; }

        /// <summary>
        /// Initializes a new GetUpdates request
        /// </summary>
        public GetUpdatesRequest()
            : base("getUpdates")
        {
        }
    }
}
