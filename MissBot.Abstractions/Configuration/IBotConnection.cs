

/* Unmerged change from project 'MissBot.Abstractions (netstandart2.0)'
Before:
using MissCore.Configuration;
After:
using MissBot.Abstractions;
using MissCore;
using MissCore.Abstractions;
using MissCore.Configuration;
*/
using Telegram.Bot.Requests.Abstractions;
using Telegram.Bot.Types;

namespace MissBot.Abstractions.Configuration
{
    public interface IBotConnection
    {
        IBotConnectionOptions Options { get; }
        uint Timeout { get; }
        Task DownloadFileAsync(string filePath, Stream destination, CancellationToken cancellationToken = default);
        Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task<TBot> GetBotAsync<TBot>(IBotConnectionOptions options, CancellationToken cancellationToken = default) where TBot:BaseBot;
    }
}

