using Microsoft.Extensions.DependencyInjection;
using MissCore.Configuration;

namespace MissCore.Abstractions
{
    public interface IHandleContextFactory
    {
        IServiceScopeFactory ScopeFactory { get; }
        IServiceScope this[string scopeHash] { get; }
        IServiceScope Init(string identifier);
    }
}
