using BotService.Connection;
using BotService.Interfaces;
using MissBot.Abstractions;
using MissBot.Attributes;
using MissBot.Extensions;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Entities;
using MissCore.Handlers;
using MissDataMaiden.Commands;
using Telegram.Bot.Types;

namespace MissDataMaiden
{
    [HasBotCommand(Name = nameof(List), Description = "List of data bases with info")]
    [HasBotCommand(Name = nameof(Info), Description = "Inforamtion about current server state")]
    [HasBotCommand(Name = nameof(Disk), Description = "Disk space information")]
    public class MissChannel : IBot<Update<MissChannel>>
    {
        public class Update : Update<MissChannel> { }
        private readonly ILogger<MissChannel> _logger;
        IServiceScope scope;
        public MissChannel(ILogger<MissChannel> logger)
        {
            _logger = logger;       
        }
        public IServiceProvider BotServices
            => scope.ServiceProvider;
        public User BotInfo { get; set; }

        public Func<global::Update, string> ScopePredicate
            => (u) => $"{nameof(u.Message.Chat)}: {u.Message.Chat.Id}";

        public void ConfigureConnection(IBotConnectionOptionsBuilder connectionOptions)
           => connectionOptions
                   .SetToken(Environment.GetEnvironmentVariable("AliseBot", EnvironmentVariableTarget.User))
                   .SetTimeout(TimeSpan.FromMinutes(2));


        public void ConfigureOptions(IBotOptionsBuilder botBuilder)
            => botBuilder.TrackMessgeChanges();

        public void SetScope(IServiceScope botScope)
            => scope = botScope;

        public IHandleContext CreateHandleContext<TUpdate>(TUpdate update) where TUpdate : IUpdateInfo
        {
            throw new NotImplementedException();
        }

        public Task CreateHandle<TUpdate>(TUpdate update) where TUpdate : IUpdateInfo
        {
            throw new NotImplementedException();
        }
    }
}
