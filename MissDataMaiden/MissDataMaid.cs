using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataContext;
using MissBot.Abstractions.Entities;
using MissBot.Entities;

namespace MissDataMaiden
{

    //[HasBotCommand(Name = nameof(List), CmdType = typeof(List), Description = "List of data bases with info")]
    //[HasBotCommand(Name = nameof(Info), CmdType = typeof(Info), Description = "Inforamtion about current server state")]
    //[HasBotCommand(Name = nameof(Disk), CmdType = typeof(Disk), Description = "Disk space information")]
    public class MissDataMaid : BaseBot
    {
        private readonly ILogger<MissDataMaid> _logger;
        private ILogger<MissDataMaid> log;

        public override Func<IUnitUpdate, string> ScopePredicate
             => (update) => update switch
             {
                 Update upd when upd.Type is UpdateType.InlineQuery => $"{upd.InlineQuery.Id}",
                 Update upd when upd.Type is not UpdateType.Unknown => $"{nameof(update.Chat)}: {update.Chat.Id}",
                 _ => null
             };

        public MissDataMaid(ILogger<MissDataMaid> logger, IBotContext context) : base(context)
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
