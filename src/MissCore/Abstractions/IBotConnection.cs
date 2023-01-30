
using MissCore.Configuration;
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

namespace MissCore.Abstractions
{
    public interface IBotConnection
    {
        IBotConnectionOptions Options { get; }
        uint Timeout { get;  }
        Task DownloadFileAsync(string filePath, Stream destination, CancellationToken cancellationToken = default);
        Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task<User> GetBotInfoAsync(IBotConnectionOptions options, CancellationToken cancellationToken = default);
    }
}

