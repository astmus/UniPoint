using MissBot.Entities;

namespace MissBot.Abstractions.Configuration
{
    public interface IBotConnection : IDataConnection
    {
        uint Timeout { get; }
        IBotConnectionOptions Options { get; set; }
        Task DownloadFileAsync(string filePath, Stream destination, CancellationToken cancellationToken = default);
        Task<TResponse> HandleQueryAsync<TResponse>(IBotRequest<TResponse> request, CancellationToken cancellationToken = default);
        Task MakeRequestAsync(IBotRequest request, CancellationToken cancellationToken = default);
        Task<TBot> GetBotAsync<TBot>(CancellationToken cancellationToken = default) where TBot : BaseBot;
    }
    public interface IDataConnection
    {

        //void InitContext(IBotDataContext dataContext);
        //IBotConnection CreateConnection(string connectionString);

        //object? GetConnectionInfo(IDataConnection dataConnection, string parameterName);
    }
}

