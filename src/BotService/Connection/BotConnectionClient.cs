using MissCore.Configuration;

namespace BotService.Connection
{
    /// <summary>
    /// A client to use the Telegram Bot API
    /// </summary>
    public class BotConnectionClient<TBot> : BotConnection, IBotClient<TBot> where TBot:IBot
    {
        public BotConnectionClient(HttpClient httpClient = null) : base(IBotClient<TBot>.Options, httpClient)
        {         
            
        }        
    }
}
