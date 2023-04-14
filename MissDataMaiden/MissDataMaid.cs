using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Attributes;
using MissCore.Entities;
using MissDataMaiden.Commands;
using Telegram.Bot.Types;

namespace MissDataMaiden
{

    //[HasBotCommand(Name = nameof(List), CmdType = typeof(List), Description = "List of data bases with info")]
    //[HasBotCommand(Name = nameof(Info), CmdType = typeof(Info), Description = "Inforamtion about current server state")]
    //[HasBotCommand(Name = nameof(Disk), CmdType = typeof(Disk), Description = "Disk space information")]
    public class MissDataMaid : BaseBot
    {
        private readonly ILogger<MissDataMaid> _logger;
        private IServiceScope scope;
        private ILogger<MissDataMaid> log;



        public override Func<ICommonUpdate, string> ScopePredicate
             => (update) => update switch {
             Update upd when upd.Type is Telegram.Bot.Types.Enums.UpdateType.InlineQuery => $"{upd.InlineQuery.Id}",  
             _=> $"{nameof(update.Chat)}: {update.Chat.Id}"
             };

        public MissDataMaid(ILogger<MissDataMaid> logger, IHostApplicationLifetime lifeTime, IRepository<BotCommand> commands) : base(commands)
        {
            log = logger;
        }

        public override void ConfigureOptions(IBotOptionsBuilder botBuilder)
            => botBuilder
                        .ReceiveCallBacks()
                        .ReceiveInlineQueries()
                        .ReceiveInlineResult()
                        .TrackMessgeChanges();

        private Task HandleError(Exception error, CancellationToken cancel)
        {
            log.LogError(error, error.Message);
            return Task.CompletedTask;
        }

        public override void ConfigureConnection(IBotConnectionOptionsBuilder connectionOptions)
            => connectionOptions
                    .SetToken(Environment.GetEnvironmentVariable("JarviseKey", EnvironmentVariableTarget.User))
                    .SetTimeout(TimeSpan.FromMinutes(2));
         }
}
