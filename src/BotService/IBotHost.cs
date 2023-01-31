using MissCore.Abstractions;
using MissCore.Configuration;

namespace BotService
{    
    public interface IBotHost
    {
        IBotBuilder<TBot> AddBot<TBot>() where TBot : class, IBot;
        void AddBotHandler<TBot>() where TBot : class, IBot;
        void Start();
    }
}
