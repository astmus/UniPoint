using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MissCore.Configuration;
using MissCore.Handlers;
using Telegram.Bot.Types;

namespace MissCore.Abstractions
{
    public interface IBot
    {
        User BotInfo { get; set; }
        void ConfigureOptions(IBotOptionsBuilder botBuilder);
        void SetScope(IServiceScope botScope);
        void ConfigureConnection(IBotConnectionOptionsBuilder connectionBuilder);
        Func<HandleDelegate> Handler { get; set; }
    }

    public interface IBot<TUpdate> : IBot where TUpdate:class,IUpdateInfo
    {
        IServiceProvider BotServices { get; }
    }
}
