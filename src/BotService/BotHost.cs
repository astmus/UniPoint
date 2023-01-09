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
    public class BotHost : IBotHost
    {
        internal static BotConnectionOptions ConnectionOptions
            => new BotConnectionOptions();
        internal static BotOptionsBuilder BotOptions { get; set; }
        internal static IHostBuilder hostBuilder;
        internal static IBotHost Default
            => new BotHost();
        public static IBotHost CreateDefault(IBotStartupConfig startupConfig, string[] args = null)
        {
            hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureHostConfiguration(config =>
            { })
            .ConfigureServices((host, services) =>
            {
                services
                    .AddApplicationServices()
                    .AddInfrastructureServices(host.Configuration)
                    .AddHostedService<BotListener>();
                services.AddSingleton<ICurrentUserService, CurrentUserService>();
                services.AddHttpClient<IBotConnection, BotConnection>();
                services.AddScoped(typeof(IContext<>), typeof(Context<>));
                services.AddScoped<IBotConnectionOptions>(sp
                    => BotOptions.Build());
                services.AddHttpContextAccessor();

                services.AddHealthChecks()
                    .AddDbContextCheck<ApplicationDbContext>();

                startupConfig.ConfigureHost(BotHost.ConnectionOptions, host.Configuration);
                startupConfig.ConfigureBot(BotOptions = new BotOptionsBuilder(BotHost.ConnectionOptions));

            }).ConfigureAppConfiguration(config =>
            {
            })
            .ConfigureLogging(log => log.AddConsole().AddDebug())
            .ConfigureHostOptions(options =>
            {


            })
            .UseConsoleLifetime(opt =>
            { });
            return BotHost.Default;
        }

        Action<IBotBuilder> configuratorDelegate;
        public IBotHost AddBot<TBot>(Action<IBotBuilder> configurator) where TBot :class, IBot
        {
            configuratorDelegate = configurator;
            hostBuilder.ConfigureServices(services =>
            {
                services
                .AddScoped<IBot, TBot>()
                .AddTransient<IBotUpdatesDispatcher<Update>, AsyncBotUpdatesDispatcher<Update>>()
                .AddTransient<IBotUpdatesReceiver<Update>, AsyncBotUpdatesReceiver<Update>>()
                .AddTransient<IBotConnection, BotConnection>()
                .AddTransient<IBotClient, BotConnection>();
            });
            return this;
        }

        public void Run()
        {
            configuratorDelegate(BotBuilder.Default);
            hostBuilder.Build().Run();
        }
    }
}
