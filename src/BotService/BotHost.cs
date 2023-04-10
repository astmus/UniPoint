using BotService.Configuration;
using BotService.Connection;
using BotService.Internal;
using MissBot;
using MissBot.Infrastructure.Persistence;
using MissCore.Configuration;
using MissCore.Data.Context;
using MissCore.Entities;
using MissCore.Handlers;
using Microsoft.Extensions.Options;
using MissBot.Infrastructure;
using MissBot.Abstractions;

namespace BotService
{
    public class BotHost : BackgroundService, IBotHost
    {

        public BotHost()
        {
        }
        List<Action> buildActions = new List<Action>();
        ILogger<BotHost> log;
        public BotHost(ILogger<BotHost> logger)
        {
            log = logger;
        }

        public IBotBuilder<TBot> AddBot<TBot>() where TBot : class, IBot
        {
            hostBuilder.ConfigureServices(services => services
                                                                                    .AddHostedService<BotClient<TBot>>()                                                                                    
                                                                                    .AddScoped<TBot>()
                                                                                    .AddScoped<IAsyncHandler<Update<TBot>>, BotUpdateHandler<TBot>>()
                                                                                    .AddScoped<IBotUpdatesDispatcher<Update<TBot>>, AsyncBotUpdatesDispatcher<Update<TBot>>>()
                                                                                    .AddScoped<IBotUpdatesReceiver<Update<TBot>>, AsyncBotUpdatesReceiver<Update<TBot>>>()
                                                                                    .AddSingleton<IBotBuilder<TBot>>(sp => BotBuilder<TBot>.Instance)
                                                                                    .AddScoped<IBotClient>(sp => sp.GetRequiredService<IBotClient<TBot>>())
                                                                                    .AddHttpClient<IBotClient<TBot>, BotConnectionClient<TBot>>(typeof(TBot).Name));
            buildActions.Add(() => BotBuilder<TBot>.Instance.Build());
            var builder = BotBuilder<TBot>.GetInstance(hostBuilder);
            builder.Services.AddScoped<IBotClient>(sp => sp.GetRequiredService<IBotClient<TBot>>());
            builder.Services.AddHttpClient<IBotClient<TBot>, BotConnectionClient<TBot>>(typeof(TBot).Name);
            return builder;
        }

        internal static IHostBuilder hostBuilder;
        internal static IBotHost BotsHost;

        public static IBotHost CreateDefault(string[] args = null)
        {
            hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureHostConfiguration(config =>
            { })
            .ConfigureServices((host, services) =>
            {
                services
                    .AddApplicationServices()
                    .AddInfrastructureServices(host.Configuration);
                services.AddHostedService<BotHost>();
                services.AddSingleton<IHandleContextFactory, HandleContextFactory>();
                services.AddHttpClient<IBotConnection, BotConnection>();
                services.AddScoped(typeof(IContext<>), typeof(Context<>));
                services.AddScoped(sp => sp.GetRequiredService<IBotConnectionOptionsBuilder>().Build());
                services.AddScoped<IBotConnectionOptionsBuilder, BotOptionsBuilder>();
                services.AddScoped<IBotOptionsBuilder>(sp => sp.GetRequiredService<IBotConnectionOptionsBuilder>());
                services.AddHttpContextAccessor();
                services.AddHealthChecks()
                    .AddDbContextCheck<ApplicationDbContext>();

            }).ConfigureAppConfiguration(config =>
            {
            })
            .ConfigureLogging(log => log.AddConsole().AddDebug())
            .ConfigureHostOptions(options =>
            {
            });
            //.UseConsoleLifetime(opt =>
            //{ });
            return BotsHost = new BotHost();
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            log.LogInformation($"BotHost start {DateTime.Now}");
            return base.StartAsync(cancellationToken);
        }

        public void Start()
        {
            hostBuilder.Build().Run();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            buildActions.ForEach(b => b());
            log.LogInformation($"BotHost started {DateTime.Now}");
            return Task.CompletedTask;
        }        
    }
}
