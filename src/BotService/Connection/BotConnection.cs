using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Entities;

namespace BotService.Connection
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

        uint IBotConnection.Timeout
            => (uint)Options.Timeout.TotalSeconds;


        public async Task<TBot> GetBotAsync<TBot>(IBotConnectionOptions options, CancellationToken cancellationToken = default) where TBot : BaseBot
        {
            Options = options;
            var info = await MakeRequestAsync<TBot>(request: new BaseParameterlessRequest<TBot>("getMe"), cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            return info;
        }

        public async Task SendCommandAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IBotRequest
        {
            await SendRequestAsync(request, cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
        }

        public async Task<TResponse> SendQueryRequestAsync<TResponse>(IBotRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
            return await MakeRequestAsync<TResponse>(request, cancellationToken: cancellationToken)
                         .ConfigureAwait(false);
        }
    }
}
