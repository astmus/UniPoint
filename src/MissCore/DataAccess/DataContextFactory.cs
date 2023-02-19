using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissCore.Data.Context;

namespace MissCore.DataAccess
{
    public class DataContextFactory : Context, IDataContextFactory
    {
        IServiceScopeFactory scopeFactory;
        public DataContextFactory(IServiceScopeFactory clientRoot)
        {
            scopeFactory = clientRoot;
        }

        public IContext<T> GetContext<T>() where T : class
            => ActivatorUtilities.GetServiceOrCreateInstance<IContext<T>>(GetScope().ServiceProvider);

        public IServiceScope GetScope()
            => scopeFactory.CreateScope();        
    }
}
