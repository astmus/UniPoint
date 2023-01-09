using Microsoft.Extensions.Configuration;
using MissCore.Configuration;

namespace MissCore.Abstractions
{
    public interface IBot
    {
        IServiceProvider BotServices { get; }
        public void ConfigureHost(IBotConnectionOptions botConnection, IConfiguration configurationBuilder);
        public void ConfigureBot(IBotOptionsBuilder botBuilder);
    }

    public interface IBot<TUpdate> : IBot where TUpdate:IUpdateInfo
    {
        void RunAsync();
    }
}
