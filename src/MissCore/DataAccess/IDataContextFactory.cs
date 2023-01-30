using Microsoft.Extensions.DependencyInjection;

namespace MissCore.DataAccess
{
    internal interface IDataContextFactory
    {
        IServiceScope GetScope();
    }
}
