using MissCore.Abstractions;
using MissCore.Configuration;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;

namespace BotService.DataAccess
{
    /// <summary>
    /// A client to use the Telegram Bot API
    /// </summary>
    public class BotConnectionClient
    {
        public BotConnectionClient(HttpClient httpClient = null)
        {
        }      
        
        //public async Task<IBotClient> GetBotClientAsync<TBot>(IBotConnectionOptions options, CancellationToken cancellationToken = default) where TBot : IBot
        //{
        //    Options = options;
        //    Info = await MakeRequestAsync(request: new ParameterlessRequest<User>("getMe"), cancellationToken: cancellationToken)
        //                .ConfigureAwait(false);

        //    return this;
        //}
    }
}
