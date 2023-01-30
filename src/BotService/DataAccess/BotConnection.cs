using MissCore.Abstractions;
using MissCore.Configuration;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace BotService.DataAccess
{
    /// <summary>
    /// A client to use the Telegram Bot API
    /// </summary>
    public class BotConnection : BaseConnection, IBotConnection, IBotClient
    {
        public BotConnection(IBotConnectionOptions options = null, HttpClient httpClient = null) : base(httpClient: httpClient)
        {
            Options = options;
        }
        public override IBotConnectionOptions Options { get; set; }
        public IBotUpdatesDispatcher Dispatcher { get; protected set; }
        public User Info { get; protected set; }
        uint IBotConnection.Timeout
            => (uint)Options.Timeout.TotalSeconds;

        public async Task<User> GetBotInfoAsync(IBotConnectionOptions options, CancellationToken cancellationToken = default)
        {
            Options = options;
            var info = await MakeRequestAsync(request: new ParameterlessRequest<User>("getMe"), cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            return info;
        }
    }
}
