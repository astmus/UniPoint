using MissBot.Entities;

namespace MissBot.Abstractions.Configuration
{
    public interface IBotClient : IBotConnection
    {
        Task SendCommandAsync<TRequest>(TRequest request, CancellationToken cancellationToken = default) where TRequest : IBotRequest;
        Task<TResponse> SendQueryRequestAsync<TResponse>(IBotRequest<TResponse> request, CancellationToken cancellationToken = default);
    }

    public interface IBotClient<TBot> : IBotClient where TBot : IBot
    {
        static IBotConnectionOptions Options { get; set; }

        // Task<User> GetBotClientAsync(CancellationToken cancellationToken = default);
    }
}

