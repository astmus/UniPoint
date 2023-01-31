using Microsoft.Extensions.DependencyInjection;
using MissCore.Abstractions;
using MissCore.Configuration;
using Telegram.Bot.Types;

namespace MissCore
{
    public abstract class Bot : IBot
    {
        public abstract User BotInfo { get; set; }
        public Func<Update, string> ScopePredicate { get; }

        public abstract void ConfigureConnection(IBotConnectionOptionsBuilder connectionBuilder);
        public abstract void ConfigureOptions(IBotOptionsBuilder botBuilder);
        public abstract Task CreateHandle<TUpdate>(TUpdate update) where TUpdate : IUpdateInfo;
        public abstract IHandleContext CreateHandleContext<TUpdate>(TUpdate update) where TUpdate : IUpdateInfo;
        public abstract void SetScope(IServiceScope botScope);
    }
}
