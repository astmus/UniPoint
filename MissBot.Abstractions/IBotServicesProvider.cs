
using MissCore.Configuration;

namespace MissBot.Abstractions
{
    public interface IBotServicesProvider : IServiceProvider
    {
        IBotClient Client { get; }
        T GetService<T>();
        T GetRequiredService<T>();
    }
}
