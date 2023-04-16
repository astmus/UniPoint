using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;

namespace BotService
{
    public interface IBotHost
    {
        IBotBuilder<TBot> AddBot<TBot, TConfig>() where TBot : BaseBot where TConfig : BaseBot.Configurator;
        void Start();
    }
}
