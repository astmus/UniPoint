using BotService.Configuration;
using BotService.DataAccess;
using BotService.Interfaces;
using BotService.Internal;
using MissBot;
using MissBot.Common.Interfaces;
using MissBot.Infrastructure;
using MissBot.Infrastructure.Persistence;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Data.Context;
using MissCore.DataAccess.Async;

namespace BotService
{
    public class BotHost : BackgroundService, IBotHost
    {

        static List<IBotBuilder> botsBuilders = new List<IBotBuilder>();
        List<IBot> bots = new List<IBot>();

        public BotHost()
        {
            int i = 0;
        }
        IServiceScopeFactory scopeFactory;
        public BotHost(IServiceScopeFactory factory)
            => scopeFactory = factory;

        public IBotBuilder<TBot> AddBot<TBot>() where TBot : class, IBot
        {
            hostBuilder.ConfigureServices(services => services.AddHostedService<BotClient<TBot>>()
                                                                                    .AddScoped<TBot>()
                                                                                    .AddSingleton<IBotBuilder<TBot>>(sp => BotBuilder<TBot>.Instance));
            botsBuilders.Add(BotBuilder<TBot>.Instance);
            return BotBuilder<TBot>.Instance;
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
                //services
                //    .AddApplicationServices()
                //    .AddInfrastructureServices(host.Configuration);
                services.AddHostedService<BotHost>();
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
            })
            .UseConsoleLifetime(opt =>
            { });
            return BotsHost = new BotHost();
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            foreach (var bot in botsBuilders)
                bots.Add(bot.BuildClient(scopeFactory.CreateScope()));
            
            return base.StartAsync(cancellationToken);
        }
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //    => await RootHost.RunAsync();
        IHost host;
        public void Start()
        {
            // bots.ForEach(b => b.BuildService());
            host = hostBuilder.Build();
            host.Run();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            
            return Task.CompletedTask;
        }

        //protected override Task ExecuteAsync(CancellationToken stoppingToken)
        //{
        //    return Task.CompletedTask;
        //}
    }
}
