using MissBot.Abstractions.Bot;

namespace MissBot.Abstractions.Configuration
{
    public interface IBot
    {
        Func<IUnitUpdate, string> ScopePredicate { get; }
        IBotServicesProvider BotServices { get; }
        Task<bool> SyncCommands();
    }

    public interface IBot<in TUpdate> : IBot where TUpdate : class, IUpdateInfo
    {
    }
}
