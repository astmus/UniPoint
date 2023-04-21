using MissBot.Abstractions.Entities;
using BotCommand = MissBot.Abstractions.Entities.BotCommand;

namespace MissBot.Abstractions.Configuration
{
    public interface IBot
    {       
        Func<ICommonUpdate, string> ScopePredicate { get; }
        IEnumerable<BotAction> Commands { get; }
    }

    public interface IBot<in TUpdate> : IBot where TUpdate : class, IUpdateInfo
    {
        IServiceProvider BotServices { get; }
    }
}
