using BotService.Configuration;
using BotService.DataAccess;
using MissBot.Extensions;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.DataAccess.Async;
using MissCore.Entities;
using Telegram.Bot.Types;

namespace BotService
{
    public class BotClient<TBot> : BackgroundService where TBot : class, IBot
    {
        private readonly ILogger<BotClient<TBot>> _logger;
        private readonly IHostApplicationLifetime _hostLifeTime;

        IHandleContextFactory factory;
        IServiceScope scope;
        IBotBuilder<TBot> builder;
        public BotClient(ILogger<BotClient<TBot>> logger, IHostApplicationLifetime hostLifeTime, IHandleContextFactory scopeFactory)
        {
            _logger = logger;            
            factory = scopeFactory;
            _hostLifeTime = hostLifeTime;
            botScopeServices = (scope = factory.ScopeFactory.CreateScope()).ServiceProvider;
        }

        IServiceProvider botScopeServices
        { get; init; }
        

        protected User info;
        TBot Bot;
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            botScopeServices.GetRequiredService<IBotBuilder<TBot>>().Build();
            Bot = botScopeServices.GetRequiredService<TBot>();
            Bot.ConfigureOptions(botScopeServices.GetRequiredService<IBotOptionsBuilder>());
            Bot.ConfigureConnection(botScopeServices.GetRequiredService<IBotConnectionOptionsBuilder>());
            var client = botScopeServices.GetRequiredService<IBotConnection>();
            try
            {
                var info = await client.GetBotInfoAsync(botScopeServices.GetRequiredService<IBotConnectionOptionsBuilder>().Build(), cancellationToken);
                Bot.BotInfo = info;                              
                _logger.LogInformation($"Worker runned at: {DateTimeOffset.Now} {info}");
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
            }
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var dispatcher = botScopeServices.GetRequiredService<IBotUpdatesDispatcher<Update<TBot>>>();
            dispatcher.ScopePredicate = Bot.ScopePredicate;
            var updatesQueue = botScopeServices.GetRequiredService<IBotUpdatesReceiver<Update<TBot>>>();

            dispatcher.Initialize(stoppingToken);
            await foreach (var update in updatesQueue.WithCancellation(stoppingToken))
                dispatcher.PushUpdate(update);

            scope.Dispose();

        }
    }
}
