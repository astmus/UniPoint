using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions.Configuration
{
    public interface IBot
    {
        Func<IUnitUpdate, string> ScopePredicate { get; }
        IEnumerable<BotCommand> Commands { get; }
        IBotServicesProvider BotServices { get; }
    }

    public interface IBot<in TUpdate> : IBot where TUpdate : class, IUpdateInfo
    {
    }
}
