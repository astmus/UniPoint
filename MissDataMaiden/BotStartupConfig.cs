using BotService.Configuration;
using BotService.Interfaces;

namespace MissDataMaiden
{
    public class BotStartupConfig : IBotStartupConfig
    {    

        public void ConfigureBot(IBotOptionsBuilder botBuilder)
            => botBuilder.ReceiveCallBacks().ReceiveInlineQueries().ReceiveInlineResult()
                        .SetTimeout(TimeSpan.FromMinutes(2))
                        .UseCustomUpdateHandler()
                        .SetToken(Environment.GetEnvironmentVariable("JarviseKey", EnvironmentVariableTarget.User))
                        .SetExceptionHandler(HandleError);

        public void ConfigureHost(IBotConnectionOptions botConnection, IConfiguration configurationBuilder)
        {
          
        }

        private Task HandleError(Exception error, CancellationToken cancelToken)
            => Task.FromException(error);
    }
}
