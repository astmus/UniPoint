using BotService.Configuration;
using BotService.Interfaces;
using MissBot.Application;
using MissBot.Application.Common.Interfaces;
using MissBot.Infrastructure.Persistence;

namespace BotService
{
    public class BotHost
    {
        internal static BotConnectionOptions ConnectionOptions
            => new BotConnectionOptions();
        internal static BotOptionsBuilder BotOptions { get; set; }
   
        public static void CreateDefault(IBotStartupConfig startupConfig, string[] args = null)
        {        
            IHostBuilder hostBuilder = Host.CreateDefaultBuilder(args);
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
            

            hostBuilder.Build().Run();
        }
    }
}
