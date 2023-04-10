using Microsoft.Extensions.DependencyInjection;

namespace MissBot.Abstractions
{
    public interface IHandleContextFactory
    {
        IServiceScopeFactory ScopeFactory { get; }
        IServiceScope this[string scopeHash] { get; }
        IServiceScope Init(string identifier);
        void Remove(string identifier);
    }
}
