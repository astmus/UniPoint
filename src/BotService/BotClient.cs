using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Extensions;
using MissCore.Data;
using MissCore.DataAccess.Async;

namespace BotService
{
    public class BotClient<TBot> : BackgroundService where TBot : BaseBot
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


        TBot Bot;
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            botScopeServices.GetRequiredService<IBotBuilder<TBot>>().Build();
            var config = botScopeServices.GetRequiredService<BaseBot.Configurator>();
            config.ConfigureOptions(botScopeServices.GetRequiredService<IBotOptionsBuilder>());
            config.ConfigureConnection(botScopeServices.GetRequiredService<IBotConnectionOptionsBuilder>());
            var client = botScopeServices.GetRequiredService<IBotConnection>();
            try
            {
                var botConnectionOptions = IBotClient<TBot>.Options = botScopeServices.GetRequiredService<IBotConnectionOptions>();
                Bot = await client.GetBotAsync<TBot>(botConnectionOptions, cancellationToken);// need fix (fast approach)
                Bot.Initialize();
                await Bot.SyncCommands(client);
                _logger.LogInformation($"Worker runned at: {DateTimeOffset.Now} {Bot}");
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
            }
            await base.StartAsync(cancellationToken).ConfigFalse();
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
