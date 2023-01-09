using MissCore.Configuration;

namespace BotService.Interfaces
{
    public interface IBotStartupConfig
    {
        public void ConfigureHost(IBotConnectionOptions botConnection, IConfiguration configurationBuilder);
        public void ConfigureBot(IBotOptionsBuilder botBuilder);
        
    }
}
