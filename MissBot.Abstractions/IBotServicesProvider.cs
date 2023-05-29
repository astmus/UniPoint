
using MissBot.Abstractions.Configuration;

namespace MissBot.Abstractions
{
    public interface IBotServicesProvider : IServiceProvider
    {
        IBotClient Client { get; }        
        IResponse<T> Response<T>();
        IResponseError ResponseError();
        T Activate<T>() where T : class;
        T GetService<T>();
        T GetRequiredService<T>();
    }
}
