using MissCore.Abstractions;
using MissCore.Configuration;

namespace BotService
{
    public interface IBotHost
    {
        void Run();
        IBotHost AddBot<TBot>(Action<IBotBuilder> configurator) where TBot :class, IBot;
    }
}
