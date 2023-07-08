using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Bot;

namespace MissBot.Abstractions.DataAccess
{
    public interface IRepository
    {
        Task ExecuteCommandAsync(IUnitRequest request, CancellationToken cancel = default);
        Task<IContentUnit<TResult>> HandleQueryAsync<TResult>(IUnitRequest request, CancellationToken cancel = default) where TResult : class;
        Task<TResult> HandleRawAsync<TResult>(string request, CancellationToken cancel = default) where TResult : class;
    }
}
