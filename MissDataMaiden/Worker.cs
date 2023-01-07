using BotService.Interfaces;
using MissBot.Attributes;
using MissCore.Abstractions;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    [HasBotCommand(Name = nameof(List), Description = "List of data bases with info")]
    [HasBotCommand(Name = nameof(Info), Description = "Inforamtion about current server state")]
    [HasBotCommand(Name = nameof(Disk), Description = "Disk space information")]
    public class BotWorker : BackgroundService, IBot
    {
        private readonly ILogger<BotWorker> _logger;

        public BotWorker(ILogger<BotWorker> logger)
        {
            _logger = logger;
        }        
        public IBotServicesProvider BotServices { get; }

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
