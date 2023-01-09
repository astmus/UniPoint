using BotService.DataAccess;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.DataAccess.Async;

namespace BotService
{
    public abstract  class BaseBot<TUpdate> : BackgroundService, IBot<TUpdate> where TUpdate:class, IUpdateInfo
    {
        private readonly ILogger<BaseBot<TUpdate>> _logger;
        private readonly IHostApplicationLifetime _hostLifeTime;
        IBotUpdatesDispatcher<TUpdate> dispatcher;
        IBotUpdatesReceiver<TUpdate> receiver;
        IServiceProvider root;
        public BaseBot(ILogger<BaseBot<TUpdate>> logger, IHostApplicationLifetime hostLifeTime, IServiceProvider rootServiceprovider)
        {
            _logger = logger;
            root = rootServiceprovider;
            _hostLifeTime = hostLifeTime;
        }

        public IServiceProvider BotServices { get; init; }

        public abstract void ConfigureBot(IBotOptionsBuilder botBuilder);
        public abstract void ConfigureHost(IBotConnectionOptions botConnection, IConfiguration configurationBuilder);
        protected abstract IBotUpdatesDispatcher<TUpdate> Dispatcher { get; }
        protected abstract IBotUpdatesReceiver<TUpdate> Receiver { get; }
        public void RunAsync()
        {
            throw new NotImplementedException();
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
                var dispatcher = Dispatcher;//scope.ServiceProvider.GetRequiredService<IBotUpdatesDispatcher<Update>>();
                var updatesQueue = Receiver;//scope.ServiceProvider.GetRequiredService<IBotUpdatesReceiver<Update>>();
                
                dispatcher.Initialize(stoppingToken);
                await foreach (var update in updatesQueue.WithCancellation(stoppingToken))
                    dispatcher.PushUpdate(update);
            }
        }
    }
}
