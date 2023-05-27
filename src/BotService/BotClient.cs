using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess.Async;
using MissBot.Extensions;
using MissCore.Data;

namespace BotService
{
    public class BotClient<TBot> : BackgroundService where TBot : BaseBot
    {
        private readonly ILogger<BotClient<TBot>> _log;
        private readonly IHostApplicationLifetime _hostLifeTime;
        TBot Bot;
        IHandleContextFactory factory;
        IServiceScope scope;
        IBotBuilder<TBot> builder;
        IServiceProvider botScopeServices { get; init; }
        public BotClient(ILogger<BotClient<TBot>> logger, IHostApplicationLifetime hostLifeTime, IHandleContextFactory scopeFactory)
        {
            _log = logger;
            factory = scopeFactory;
            _hostLifeTime = hostLifeTime;
            botScopeServices = (scope = factory.ScopeFactory.CreateScope()).ServiceProvider;
        }
        
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _log.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            botScopeServices.GetRequiredService<IBotBuilder<TBot>>().Build();

            var config = botScopeServices.GetRequiredService<BaseBot.Configurator>();
            config.ConfigureOptions(botScopeServices.GetRequiredService<IBotOptionsBuilder>());
            config.ConfigureConnection(botScopeServices.GetRequiredService<IBotConnectionOptionsBuilder>());
            //botScopeServices.GetRequiredService<IBotConnectionOptionsBuilder>().SetExceptionHandler(

            try
            {
                var client = botScopeServices.GetRequiredService<IBotConnection>();
                var botConnectionOptions = IBotClient<TBot>.Options = botScopeServices.GetRequiredService<IBotConnectionOptions>();
                Bot = await client.GetBotAsync<TBot>(cancellationToken);
                Bot.Initialize(scope);
                await Bot.SyncCommands();
                _log.LogInformation($"Worker runned at: {DateTimeOffset.Now} {Bot}");
            }
            catch (Exception ex)
            {
                _log.WriteError(ex);
                Console.ReadKey();
                await StopAsync(cancellationToken);
                _hostLifeTime.StopApplication();
            }

            if (!_hostLifeTime.ApplicationStopping.IsCancellationRequested && !_hostLifeTime.ApplicationStopped.IsCancellationRequested)
                await base.StartAsync(cancellationToken).ConfigFalse();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var dispatcher = botScopeServices.GetRequiredService<IBotUpdatesDispatcher<Update<TBot>>>();
                dispatcher.ScopePredicate = Bot.ScopePredicate;
                var updatesQueue = botScopeServices.GetRequiredService<IBotUpdatesReceiver<Update<TBot>>>();

                dispatcher.Initialize(stoppingToken);
                await foreach (var update in updatesQueue.WithCancellation(stoppingToken))
                    dispatcher.PushUpdate(update);
            }
            catch (Exception error)
            {
                _log.WriteCritical(error);
                Console.ReadKey();
                scope.Dispose();
            }
        }
    }
}
