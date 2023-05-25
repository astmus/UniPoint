using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.DataAccess
{
    public interface IBotRepository : IRepository
    {
        string Name { get; }
        int ID { get; }
        string ConnectionNamespace { get; }
        IDataConnection DataProvider { get; }
        IEnumerable<BotCommand> Commands { get; }
    }

    public interface IRepository
    {
        Task ExecuteCommandAsync(IUnitRequest request, CancellationToken cancel = default);
        Task<TResult> HandleQueryAsync<TResult>(IUnitRequest request, CancellationToken cancel = default) where TResult : class;
        Task<TResult> HandleRawAsync<TResult>(string request, CancellationToken cancel = default) where TResult : class;
        //Task<TResult> HandleCommandAsync<TResult>(IRepositoryCommand query, CancellationToken cancel = default);
        //Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(IQueryCommand query, CancellationToken cancel = default);
    }
}
