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
        void ConfigureConnection(IBotConnectionOptionsBuilder connectionBuilder);
        Func<Update, string> ScopePredicate { get; }
        //IAsyncHandler<TUpdate> CreateHandleContext<TUpdate>(TUpdate update) where TUpdate:IUpdateInfo;
        //Task CreateHandle<TUpdate>(TUpdate update) where TUpdate : IUpdateInfo;
    }

    public interface IBot<in TUpdate> : IBot where TUpdate:class,IUpdateInfo
    {
        IServiceProvider BotServices { get; }
    }
}
