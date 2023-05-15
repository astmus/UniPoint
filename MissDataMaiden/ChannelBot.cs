using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Attributes;
using MissCore.Data;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    [HasBotCommand(Name = nameof(List), Description = "List of data bases with info")]
    [HasBotCommand(Name = nameof(Info), Description = "Inforamtion about current server state")]
    [HasBotCommand(Name = nameof(Disk), Description = "Disk space information")]
    public class MissChannel : BaseBot, IBot<Update<MissChannel>>
    {

        public class Update : Update<MissChannel> { }
        private readonly ILogger<MissChannel> _logger;

        public MissChannel(ILogger<MissChannel> logger, IBotContext botDataContext) : base(botDataContext)
        {
            _logger = logger;
        }

        public override Func<ICommonUpdate, string> ScopePredicate
            => (u) => $"{nameof(u.Message.Chat)}: {u.Message.Chat.Id}";

        public void ConfigureConnection(IBotConnectionOptionsBuilder connectionOptions)
           => connectionOptions
                   .SetToken(Environment.GetEnvironmentVariable("AliseBot", EnvironmentVariableTarget.User))
                   .SetTimeout(TimeSpan.FromMinutes(2));

        public void ConfigureOptions(IBotOptionsBuilder botBuilder)
            => botBuilder.TrackMessgeChanges();   
    }
}
