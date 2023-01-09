using MissCore.Configuration;
using Telegram.Bot.Requests.Abstractions;

namespace BotService.DataAccess
{
    public interface IBotConnection
    {
        IBotConnectionOptions Options { get; }
        uint Timeout { get;  }
        Task DownloadFileAsync(string filePath, Stream destination, CancellationToken cancellationToken = default);
        Task<TResponse> MakeRequestAsync<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task<bool> TestApiAsync(CancellationToken cancellationToken = default);

        //IBotClient SetupContext(IHandleContext context);
        //IBotChannel Channel { get; }
    }
}

