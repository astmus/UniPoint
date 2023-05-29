using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.DataAccess
{
    public interface IRepository
    {
        Task ExecuteCommandAsync(IUnitRequest request, CancellationToken cancel = default);
        Task<TResult> HandleQueryAsync<TResult>(IUnitRequest request, CancellationToken cancel = default) where TResult : class;
        Task<TResult> HandleRawAsync<TResult>(string request, CancellationToken cancel = default) where TResult : class;
    }
}
