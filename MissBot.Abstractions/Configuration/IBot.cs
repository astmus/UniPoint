using MissBot.Abstractions;


using Telegram.Bot.Types;

namespace MissCore.Configuration
{
    public interface IBot
    {
        User BotInfo { get; set; }
        void ConfigureOptions(IBotOptionsBuilder botBuilder);
        void ConfigureConnection(IBotConnectionOptionsBuilder connectionBuilder);
        Func<ICommonUpdate, string> ScopePredicate { get; }
    }

    public interface IBot<in TUpdate> : IBot where TUpdate : class, IUpdateInfo
    {
        IServiceProvider BotServices { get; }
    }
}
