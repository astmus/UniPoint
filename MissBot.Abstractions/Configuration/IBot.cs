using MissBot.Abstractions;
using Telegram.Bot.Types;

namespace MissBot.Abstractions.Configuration
{
    public interface IBot
    {       
        Func<ICommonUpdate, string> ScopePredicate { get; }
        IEnumerable<BotCommand> Commands { get; }
    }

    public interface IBot<in TUpdate> : IBot where TUpdate : class, IUpdateInfo
    {
        IServiceProvider BotServices { get; }
    }
}
