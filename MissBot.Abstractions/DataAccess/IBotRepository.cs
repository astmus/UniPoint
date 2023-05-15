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
        Task ExecuteCommandAsync(IRepositoryCommand query, CancellationToken cancel = default);
        Task<TResult> HandleQueryAsync<TResult>(IRepositoryCommand query, CancellationToken cancel = default) where TResult : class;
        //Task<TResult> HandleCommandAsync<TResult>(IRepositoryCommand query, CancellationToken cancel = default);
        //Task<ICollection<TResult>> HandleQueryItemsAsync<TResult>(IQueryCommand query, CancellationToken cancel = default);
    }
}
