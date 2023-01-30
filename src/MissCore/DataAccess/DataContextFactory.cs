using Microsoft.Extensions.DependencyInjection;
using MissCore.Abstractions;
using MissCore.Data.Context;

namespace MissCore.DataAccess
{


    public class DataContextFactory : Context, IDataContextFactory
    {
        IServiceScopeFactory scopeFactory;


        public DataContextFactory(IServiceScopeFactory clientRoot) : base(null)
        {
            scopeFactory = clientRoot;
          //  mainScope = clientRoot.CreateScope();
            //Services = mainScope.ServiceProvider;
        }
        public IContext<T> GetContext<T>() where T : class
            => ActivatorUtilities.GetServiceOrCreateInstance<IContext<T>>(Services);
        
        public IServiceScope GetScope(string scopeId)
            => Get<IServiceScope>(scopeId) ?? Set(Services.CreateScope(), scopeId);
      
        public T GetOrInit<T>() where T : class
            => ActivatorUtilities.GetServiceOrCreateInstance<T>(Services);

        public IServiceScope GetScope()
            => scopeFactory.CreateScope();
        
    }
}
