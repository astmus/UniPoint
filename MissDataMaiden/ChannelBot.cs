using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissCore.Data;

namespace MissDataMaiden
{
    public class MissChannel : BaseBot, IBot<Update<MissChannel>>
    {
        public class Update : Update<MissChannel> { }
        private readonly ILogger<MissChannel> _logger;

        public MissChannel(ILogger<MissChannel> logger, IBotContext botDataContext) : base(botDataContext)
        {
            _logger = logger;
        }

        public override Func<IUnitUpdate, string> ScopePredicate
            => (u) => $"{nameof(u.CurrentMessage.Chat)}: {u.CurrentMessage.Chat.Id}";

        public void ConfigureConnection(IBotConnectionOptionsBuilder connectionOptions)
           => connectionOptions
                   .SetToken(Environment.GetEnvironmentVariable("AliseBot", EnvironmentVariableTarget.User))
                   .SetTimeout(TimeSpan.FromMinutes(2));

        public void ConfigureOptions(IBotOptionsBuilder botBuilder)
            => botBuilder.TrackMessgeChanges();

        protected override void LoadBotInfrastructure()
        {
            throw new NotImplementedException();
        }
    }
}
