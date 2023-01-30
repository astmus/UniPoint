using BotService.Common;
using MissBot.Attributes;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Handlers;
using MissDataMaiden.Commands;
using Telegram.Bot.Types;

namespace MissDataMaiden
{
    public class MissDataMaidUpdate : Update<MissDataMaid>
    { }

    [HasBotCommand(Name = nameof(List), Description = "List of data bases with info")]
    [HasBotCommand(Name = nameof(Info), Description = "Inforamtion about current server state")]
    [HasBotCommand(Name = nameof(Disk), Description = "Disk space information")]
    public class MissDataMaid :  IBot<Update<MissDataMaid>>
    {
        private readonly ILogger<MissDataMaid> _logger;
        private IServiceScope scope;

        public IServiceProvider BotServices
            => scope.ServiceProvider;
        public User BotInfo { get; set; }
        public Func<HandleDelegate> Handler { get; set; }

        public MissDataMaid(ILogger<MissDataMaid> logger, IHostApplicationLifetime lifeTime)
        {                       
        }

        public void ConfigureOptions(IBotOptionsBuilder botBuilder)
            => botBuilder 
                        .ReceiveCallBacks()
                        .ReceiveInlineQueries()
                        .ReceiveInlineResult();                        

        private Task HandleError(Exception error, CancellationToken cancel)
        {
            throw new NotImplementedException();
        }   

        public void ConfigureConnection(IBotConnectionOptionsBuilder connectionOptions)
            => connectionOptions            
                    .SetToken(Environment.GetEnvironmentVariable("JarviseKey", EnvironmentVariableTarget.User))
                    .SetTimeout(TimeSpan.FromMinutes(2))                        
                    .SetExceptionHandler(HandleError);

        public void SetScope(IServiceScope botScope)
            => scope = botScope;
    }
}
