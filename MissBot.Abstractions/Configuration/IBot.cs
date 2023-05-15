using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.Configuration
{
    public interface IBot
    {
        Func<ICommonUpdate, string> ScopePredicate { get; }
        IEnumerable<BotCommand> Commands { get; }
        IServiceProvider BotServices { get; }
    }

    public interface IBot<in TUpdate> : IBot where TUpdate : class, IUpdateInfo
    {
    }
}
