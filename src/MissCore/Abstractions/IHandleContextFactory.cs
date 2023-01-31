using Microsoft.Extensions.DependencyInjection;
using MissCore.Configuration;

namespace MissCore.Abstractions
{
    public interface IHandleContextFactory
    {
        IServiceScopeFactory ScopeFactory { get; }
        IServiceScope this[string scopeHash] { get; }
        IHandleContext Creatre<TUpdate>(TUpdate update) where TUpdate : IUpdateInfo;
        IServiceScope Init(string identifier);
    }
}
