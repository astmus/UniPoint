using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Attributes;
using MissCore.Bot;
using MissCore.Entities;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{

    //[HasBotCommand(Name = nameof(List), CmdType = typeof(List), Description = "List of data bases with info")]
    //[HasBotCommand(Name = nameof(Info), CmdType = typeof(Info), Description = "Inforamtion about current server state")]
    //[HasBotCommand(Name = nameof(Disk), CmdType = typeof(Disk), Description = "Disk space information")]
    public class MissDataMaid : BaseCoreBot
    {
        private readonly ILogger<MissDataMaid> _logger;
        private IServiceScope scope;
        private ILogger<MissDataMaid> log;  

        public override Func<ICommonUpdate, string> ScopePredicate
             => (update) => update switch
             {
                 Update upd when upd.Type is Telegram.Bot.Types.Enums.UpdateType.InlineQuery => $"{upd.InlineQuery.Id}",
                 _ => $"{nameof(update.Chat)}: {update.Chat.Id}"
             };



        public MissDataMaid(ILogger<MissDataMaid> logger, IHostApplicationLifetime lifeTime, IRepository<BotCommand> commands) : base(commands)
        {
            log = logger;
        }



        private Task HandleError(Exception error, CancellationToken cancel)
        {
            log.LogError(error, error.Message);
            return Task.CompletedTask;
        }


    }

    public class MissDataMaidConfigurator : MissDataMaid.Configurator
    {
        public override void ConfigureOptions(IBotOptionsBuilder botBuilder)
            => botBuilder
                        .ReceiveCallBacks()
                        .ReceiveInlineQueries()
                        .ReceiveInlineResult()
                        .TrackMessgeChanges();

        public override void ConfigureConnection(IBotConnectionOptionsBuilder connectionOptions)
            => connectionOptions
                    .SetToken(Environment.GetEnvironmentVariable("AliseBot", EnvironmentVariableTarget.User))
                    .SetTimeout(TimeSpan.FromMinutes(2));
    }
}
