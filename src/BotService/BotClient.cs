using BotService.Configuration;
using BotService.DataAccess;
using MissBot.Extensions;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.DataAccess.Async;
using Telegram.Bot.Types;

namespace BotService
{
    public class BotClient<TBot> : BackgroundService where TBot : class, IBot
    {
        private readonly ILogger<BotClient<TBot>> _logger;
        private readonly IHostApplicationLifetime _hostLifeTime;

        IServiceScope scope;
        IBotBuilder<TBot> builder;
        public BotClient(ILogger<BotClient<TBot>> logger, IHostApplicationLifetime hostLifeTime, IServiceScopeFactory scopeFactory, IBotBuilder<TBot> botBuilder)
        {
            _logger = logger;
            builder = botBuilder;
            scope = scopeFactory.CreateScope();
            _hostLifeTime = hostLifeTime;
        }

        IServiceProvider BotServices
            => scope.ServiceProvider;

        protected User info;
        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            var bot = BotServices.GetRequiredService<TBot>();
            bot.ConfigureOptions(BotServices.GetRequiredService<IBotOptionsBuilder>());
            bot.ConfigureConnection(BotServices.GetRequiredService<IBotConnectionOptionsBuilder>());
            var client = BotServices.GetRequiredService<IBotConnection>();
            try
            {
                var info = await client.GetBotInfoAsync(BotServices.GetRequiredService<IBotConnectionOptionsBuilder>().Build(), cancellationToken);
                bot.BotInfo = info;
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
            var dispatcher = ActivatorUtilities.GetServiceOrCreateInstance<AsyncBotUpdatesDispatcher<Update<TBot>>>(scope.ServiceProvider);
            var updatesQueue = ActivatorUtilities.GetServiceOrCreateInstance<AsyncBotUpdatesReceiver<Update<TBot>>>(scope.ServiceProvider);

            dispatcher.Initialize(stoppingToken);
            await foreach (var update in updatesQueue.WithCancellation(stoppingToken))
                dispatcher.PushUpdate(update);

            scope.Dispose();

        }
    }
}
