using BotService.Configuration;
using BotService.DataAccess;
using BotService.Internal;
using MissBot;
using MissBot.Infrastructure.Persistence;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Data.Context;
using MissCore.DataAccess.Async;
using MissCore.Entities;
using MissCore.Handlers;

namespace BotService
{
    public class BotHost : BackgroundService, IBotHost
    {

        public BotHost()
        {
        }
        List<Action> builders = new List<Action>();
        ILogger<BotHost> log;
        public BotHost(ILogger<BotHost> logger)
        {
            log = logger;
        }

        public IBotBuilder<TBot> AddBot<TBot>() where TBot : class, IBot
        {
            hostBuilder.ConfigureServices(services => services
                                                                                    .AddHostedService<BotClient<TBot>>()
                                                                                    .AddTransient<IBotHandler<TBot>, BotHandler<TBot>>()
                                                                                    .AddScoped<TBot>()
                                                                                    .AddScoped<IAsyncHandler<Update<TBot>>, Handler<TBot>>()
                                                                                    .AddScoped<IBotUpdatesDispatcher<Update<TBot>>, AsyncBotUpdatesDispatcher<Update<TBot>>>()
                                                                                    .AddScoped<IBotUpdatesReceiver<Update<TBot>>, AsyncBotUpdatesReceiver<Update<TBot>>>()
                                                                                    .AddSingleton<IBotBuilder<TBot>>(sp => BotBuilder<TBot>.Instance));
            builders.Add(() => BotBuilder<TBot>.Instance.Build());
            return BotBuilder<TBot>.GetInstance();
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
                    .AddApplicationServices();
                //    .AddInfrastructureServices(host.Configuration);
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
            })
            .UseConsoleLifetime(opt =>
            { });
            return BotsHost = new BotHost();
        }


        public override Task StartAsync(CancellationToken cancellationToken)
        {
            log.LogInformation($"BotHost start {DateTime.Now}");
            return base.StartAsync(cancellationToken);
        }
        //protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        //    => await RootHost.RunAsync();
        IHost host;
        public void Start()
        {
            host = hostBuilder.Build();
            host.Run();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            builders.ForEach(b => b());
            log.LogInformation($"BotHost started {DateTime.Now}");
            return Task.CompletedTask;
        }

        public void AddBotHandler<TBot>() where TBot : class, IBot
        {
            hostBuilder.ConfigureServices(services
                                                     => services.AddScoped<IBotHandler<TBot>, BotHandler<TBot>>());
        }
    }
}
