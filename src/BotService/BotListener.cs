namespace BotService
{
    public partial class BotListener : BackgroundService
    {
        private readonly ILogger<BotListener> _logger;
        private readonly IHostApplicationLifetime _hostLifeTime;

        public BotListener(ILogger<BotListener> logger, IHostApplicationLifetime hostLifeTime)
        {
            _logger = logger;
            _hostLifeTime = hostLifeTime;
        }
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            await base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
