using BotService.Configuration;
using MissCore.Abstractions;

namespace BotService
{
    public interface IBotHost
    {
        void Run();
        IBotHost AddBot<TUpdate>(Action<IBotBuilder> configurator) where TUpdate : Update, IUpdateInfo;
    }
}
