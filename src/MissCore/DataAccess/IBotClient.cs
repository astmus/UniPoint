
using Telegram.Bot.Types;

namespace MissCore.Abstractions
{
    public interface IBotClient : IBotConnection
    {
        User Info { get; }
        IBotUpdatesDispatcher Dispatcher { get; }        
        // Task<User> GetBotClientAsync(CancellationToken cancellationToken = default);
    }

    public interface IBotClient<TBot> where TBot:IBot
    {
        User Info { get; }
        IBotUpdatesDispatcher Dispatcher { get; }
        // Task<User> GetBotClientAsync(CancellationToken cancellationToken = default);
    }
}

