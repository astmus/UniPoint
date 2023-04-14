using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;

namespace BotService
{
    public interface IBotHost
    {
        IBotBuilder<TBot> AddBot<TBot>() where TBot : BaseBot;
        void Start();
    }
}
