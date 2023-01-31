using System.Collections.Concurrent;
using Microsoft.Extensions.DependencyInjection;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Data.Context;


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

        public IHandleContext Creatre<TUpdate>(TUpdate update) where TUpdate : IUpdateInfo
        {
            throw new NotImplementedException();
        }

        public IServiceScope Init(string identifier)
            => scopes.GetOrAdd(identifier, (id) => ScopeFactory.CreateScope());
    }
}
