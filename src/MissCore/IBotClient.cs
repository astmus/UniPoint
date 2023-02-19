using MissBot.Abstractions;
using MissCore.Configuration;
using Telegram.Bot.Requests.Abstractions;

namespace MissCore.Abstractions
{
    public interface IBotClient : IBotConnection
    {
        Task SendCommandAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest:IRequest;
        Task<TResponse> SendQueryRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
    }

    public interface IBotClient<TBot> : IBotClient where TBot:IBot
    {
        static IBotConnectionOptions Options { get; set; }
        // Task<User> GetBotClientAsync(CancellationToken cancellationToken = default);
    }
}

