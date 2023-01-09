using BotService;
using BotService.DataAccess;
using BotService.Interfaces;
using MissBot.Attributes;
using MissBot.Extensions;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    [HasBotCommand(Name = nameof(List), Description = "List of data bases with info")]
    [HasBotCommand(Name = nameof(Info), Description = "Inforamtion about current server state")]
    [HasBotCommand(Name = nameof(Disk), Description = "Disk space information")]
    public class MissDataMaid : BaseBot<Update<MissDataMaid>>, IBot
    {
        private readonly ILogger<MissDataMaid> _logger;

        public MissDataMaid(ILogger<MissDataMaid> logger, IServiceProvider sp, IHostApplicationLifetime lifeTime):base(logger,lifeTime, sp)
        {           
            
        }                
        User info;
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
           var client =  BotServices.GetRequiredService<IBotClient>();
            try
            {
                info = await client.GetBotInfoAsync(cancellationToken);
            }
            catch (Exception ex)
            {
                _logger.WriteError(ex);
            }
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

        public void ConfigureHost(IBotConnectionOptions botConnection, IConfiguration configurationBuilder)
        {
            throw new NotImplementedException();
        }

        public void ConfigureBot(IBotOptionsBuilder botBuilder)
        {
            throw new NotImplementedException();
        }
    }
}
