using MissBot.Abstractions;

namespace BotService
{
    public class BotListener : BackgroundService
    {
        private readonly ILogger<BotListener> _logger;
        private readonly IHostApplicationLifetime _hostLifeTime;
        IServiceProvider root;
        public BotListener(ILogger<BotListener> logger, IHostApplicationLifetime hostLifeTime, IServiceProvider rootServiceprovider)
        {
            _logger = logger;
            root = rootServiceprovider;
            _hostLifeTime = hostLifeTime;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await base.StartAsync(cancellationToken);
            _logger.LogInformation("Worker runned at: {time}", DateTimeOffset.Now);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            using (var scope = root.CreateScope())
            {
                var dispatcher=  scope.ServiceProvider.GetRequiredService<IBotUpdatesDispatcher<Update>>();
                var updatesQueue = scope.ServiceProvider.GetRequiredService<IBotUpdatesReceiver<Update>>();
                
                dispatcher.Initialize(stoppingToken);
                await foreach (var update in updatesQueue.WithCancellation(stoppingToken))
                    dispatcher.PushUpdate(update);
            }
        }
    }
}
