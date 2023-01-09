using BotService.Interfaces;
using MissCore.Abstractions;

namespace BotService.DataAccess
{
    public interface IBotClient : IBotConnection
    {
        IBotClient SetupContext(IHandleContext context);
        Task<User> GetBotInfoAsync(CancellationToken cancellationToken = default);
    }
}

