using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;

namespace MissCore
{
    public class BotServicesProvider : IBotServicesProvider
    {
        IServiceProvider sp;
        public BotServicesProvider(IServiceProvider spr)
            => sp = spr;

        public IBotClient Client
            => GetRequiredService<IBotClient>();

        public IResponseError ResponseError()
            => sp.GetRequiredService<IResponseError>();

        public T GetRequiredService<T>()
            => sp.GetRequiredService<T>();

        public T GetService<T>()
            => sp.GetService<T>();

        public object? GetService(Type serviceType)
            => sp.GetService(serviceType);

        public IResponse<T> Response<T>()
            => sp.GetRequiredService<IResponse<T>>();

        public T Activate<T>() where T : class
            => ActivatorUtilities.GetServiceOrCreateInstance<T>(sp);
    }
}
