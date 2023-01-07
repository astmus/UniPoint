using BotService.Configuration;
using BotService.Interfaces;
using BotService.Internal;
using MissBot;
using MissBot.Common.Interfaces;
using MissBot.Infrastructure;
using MissBot.Infrastructure.Persistence;

namespace BotService
{
    public class BotHost : IBotHost
    {
        internal static BotConnectionOptions ConnectionOptions
            => new BotConnectionOptions();
        internal static BotOptionsBuilder BotOptions { get; set; }
        internal static IHostBuilder hostBuilder;
        public static IBotBuilder CreateDefault(IBotStartupConfig startupConfig, string[] args = null)
        {
            hostBuilder = Host.CreateDefaultBuilder(args);
            hostBuilder.ConfigureHostConfiguration(config =>
            {
            }).ConfigureServices((host, services) =>
            {
                services
                    .AddApplicationServices()
                    .AddInfrastructureServices(host.Configuration)
                    .AddHostedService<BotListener>();
                services.AddSingleton<ICurrentUserService, CurrentUserService>();


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


            return BotBuilder.Default;
        }

        public void RunBot()
            => hostBuilder.Build().Run();
    }
}
