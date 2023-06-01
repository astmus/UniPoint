
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions
{
    public interface IBotServicesProvider : IServiceProvider
    {
        IBotClient Client { get; }        
        IResponse<T> Response<T>() where T:BaseUnit;
        IResponseError ResponseError();
        IEnumerable<object?> GetServices(Type serviceType);
        T Activate<T>() where T : class;
        T GetService<T>();
        T GetRequiredService<T>();
    }
}
