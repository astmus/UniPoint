using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;

namespace MissCore.Handlers
{
    public class HandleContextFactory : IHandleContextFactory
    {
        public IServiceScopeFactory ScopeFactory { get; init; }

        public IServiceScope this[string scopeHash]
            => scopes[scopeHash];

        ConcurrentDictionary<string, IServiceScope> scopes = new ConcurrentDictionary<string, IServiceScope>();

        public HandleContextFactory(IServiceScopeFactory clientRoot)
        {
            ScopeFactory = clientRoot;
        }

        public IServiceScope Init(string identifier)
            => scopes.GetOrAdd(identifier, (id) => ScopeFactory.CreateScope());

        public void Remove(string identifier)
        {
            IServiceScope scope;
            scopes.Remove(identifier, out scope);
            scope.Dispose();
        }
    }
}
