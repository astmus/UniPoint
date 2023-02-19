
namespace MissBot.Abstractions
{
    public interface IBotServicesProvider : IServiceProvider
    {
        T GetService<T>();
        T GetRequiredService<T>();
    }
}
