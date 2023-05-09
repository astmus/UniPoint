using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using Telegram.Bot.Requests;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

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
        public IBotConnectionOptions options { get; set; }

        uint IBotConnection.Timeout
            => (uint)Options.Timeout.TotalSeconds;

        IConnectionOptions IDataConnection.Options { get; }

        public async Task<TBot> GetBotAsync<TBot>(IBotConnectionOptions options, CancellationToken cancellationToken = default) where TBot:BaseBot
        {
            Options = options;
            var info = await MakeRequestAsync<TBot>(request: new ParameterlessRequest<TBot>("getMe"), cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
            return info;
        }

        public async Task SendCommandAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IRequest
        {
            await SendRequestAsync(request, cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
        }

        public async Task<TResponse> SendQueryRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default)
        {
           return  await MakeRequestAsync<TResponse>(request, cancellationToken: cancellationToken)
                        .ConfigureAwait(false);
        }
    }
}
