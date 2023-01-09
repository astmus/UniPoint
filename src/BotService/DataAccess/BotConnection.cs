using BotService.Configuration;
using MediatR;
using MissCore.Abstractions;

namespace BotService.DataAccess
{
    /// <summary>
    /// A client to use the Telegram Bot API
    /// </summary>
    public class BotConnection : BaseConnection, IBotConnection, IBotClient
    {
        public BotConnection(IBotConnectionOptions botOptions, HttpClient httpClient = null) : base(botOptions, httpClient)
        {
        }

        public IBotConnectionOptions Options
            => _options;

        uint IBotConnection.Timeout
            => (uint)Options.Timeout.TotalSeconds;

        public IBotClient SetupContext(IHandleContext context)
        {
            throw new NotImplementedException();
        }
    }
}
