using MediatR;
using MissCore.Abstractions;
using MissCore.Configuration;
using Telegram.Bot.Requests;

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

        public async Task<User> GetBotInfoAsync(CancellationToken cancellationToken = default)
        => await MakeRequestAsync<User>(request: new ParameterlessRequest<User>("getMe"), cancellationToken: cancellationToken)
                    .ConfigureAwait(false);           
        

        public IBotClient SetupContext(IHandleContext context)
        {
            throw new NotImplementedException();
        }
    }
}
