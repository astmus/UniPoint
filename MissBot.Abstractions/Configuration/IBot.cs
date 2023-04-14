using MissBot.Abstractions;
using Telegram.Bot.Types;

namespace MissBot.Abstractions.Configuration
{
    public interface IBot
    {
        void ConfigureOptions(IBotOptionsBuilder botBuilder);
        void ConfigureConnection(IBotConnectionOptionsBuilder connectionBuilder);
        Func<ICommonUpdate, string> ScopePredicate { get; }
    }

    public interface IBot<in TUpdate> : IBot where TUpdate : class, IUpdateInfo
    {
        IServiceProvider BotServices { get; }
    }
}
