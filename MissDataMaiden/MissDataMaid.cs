using MissBot.Abstractions;
using MissBot.Attributes;
using MissCore.Configuration;
using MissCore.Entities;
using MissDataMaiden.Commands;
using Telegram.Bot.Types;

namespace MissDataMaiden
{

    [HasBotCommand(Name = nameof(List), CmdType = typeof(List), Description = "List of data bases with info")]
    [HasBotCommand(Name = nameof(Info), CmdType = typeof(Info), Description = "Inforamtion about current server state")]
    [HasBotCommand(Name = nameof(Disk), CmdType = typeof(Disk), Description = "Disk space information")]
    public class MissDataMaid : IBot
    {
        private readonly ILogger<MissDataMaid> _logger;
        private IServiceScope scope;
        private ILogger<MissDataMaid> log;


        public User BotInfo { get; set; }
        public Func<ICommonUpdate, string> ScopePredicate
             => (u) => u is Update<MissDataMaid> upd ? $"{nameof(upd.Chat)}: {upd.Chat.Id}" : "";

        public MissDataMaid(ILogger<MissDataMaid> logger, IHostApplicationLifetime lifeTime)
        {
            log = logger;
        }

        public void ConfigureOptions(IBotOptionsBuilder botBuilder)
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

        public void ConfigureConnection(IBotConnectionOptionsBuilder connectionOptions)
            => connectionOptions
                    .SetToken(Environment.GetEnvironmentVariable("JarviseKey", EnvironmentVariableTarget.User))
                    .SetTimeout(TimeSpan.FromMinutes(2));
         }
}
