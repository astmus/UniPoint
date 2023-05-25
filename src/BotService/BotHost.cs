using BotService.Configuration;
using BotService.Connection;
using BotService.Internal;
using MissBot;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Infrastructure;
using MissBot.Infrastructure.Persistence;
using MissBot.Utils;
using MissCore;
using MissCore.Bot;
using MissCore.Data;
using MissCore.Data.Context;
using MissCore.DataAccess;
using MissCore.DataAccess.Async;
using MissCore.Handlers;
using Newtonsoft.Json;

namespace BotService
{
    public class BotHost : HostBuilder, IBotHost
    {
        public IBotBuilder<TBot> AddBot<TBot, TConfig>() where TBot : BaseBot where TConfig : BaseBot.Configurator
        {
            var botBuilder = BotBuilder<TBot>.GetInstance(this);
            ConfigureServices((ctx, services) => services
                                                                                    .AddHostedService<BotClient<TBot>>()
                                                                                    .AddSingleton<IRequestProvider, RequestProvider>()
                                                                                    //.AddSingleton<TBot>()
                                                                                    .AddSingleton<IBotContext, BotContext>()
                                                                                    .AddSingleton<IBotBuilder<TBot>>(sp => botBuilder)
                                                                                    .AddSingleton<BaseBot.Configurator, TConfig>()
                                                                                    .AddScoped<IErrorResponse, ErrorResponse>()
                                                                                    .AddScoped<IContext<Update<TBot>>, Context<Update<TBot>>>()
                                                                                    .AddScoped<IAsyncUpdateHandler<Update<TBot>>, BotUpdateHandler<TBot>>()
                                                                                    .AddScoped<IBotUpdatesDispatcher<Update<TBot>>, AsyncBotUpdatesDispatcher<Update<TBot>>>()
                                                                                    .AddScoped<IBotUpdatesReceiver<Update<TBot>>, AsyncBotUpdatesReceiver<Update<TBot>>>()
                                                                                    .AddScoped<IBotClient>(sp => sp.GetRequiredService<IBotClient<TBot>>())
                                                                                    .AddTransient<JsonConverter, BotConverter<TBot>>()
                                                                                    .AddTransient<IBotUnitFormatProvider, BotUnitFormatProvider>()                                                                                    
                                                                                     .AddScoped(sp
                                                                                        => sp.GetRequiredService<IContext<Update<TBot>>>() as IHandleContext)
                                                                                    .AddHttpClient<IBotClient<TBot>, BotConnectionClient<TBot>>(typeof(TBot).Name));
            ConfigureServices((ctx, services) =>
                                                                services.AddOptions<BotContextOptions>().Bind(ctx.Configuration.GetSection(BotContextOptions.ContextOptions)));
            hostBuilder = this;
            return botBuilder;
        }

        internal IBotHost hostBuilder;

        public static IBotHost CreateDefault(string[] args = null)
        {
            var hostBuilder = new BotHost();
            hostBuilder.ConfigureDefaults(args);
            
            hostBuilder.ConfigureHostConfiguration(config =>
            { })
            .ConfigureServices((host, services) =>
            {
                services
                    .AddApplicationServices()
                    .AddInfrastructureServices(host.Configuration);
                services.AddSingleton<IHandleContextFactory, HandleContextFactory>();
                services.AddHttpClient<IBotConnection, BotConnection>();
                
                services.AddScoped(sp
                    => sp.GetRequiredService<IBotConnectionOptionsBuilder>().Build());
                services.AddScoped<IBotConnectionOptionsBuilder, BotOptionsBuilder>();

                services.AddScoped<IBotOptionsBuilder>(sp
                    => sp.GetRequiredService<IBotConnectionOptionsBuilder>());
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
            return hostBuilder;
        }

        public void Start()
        {
            this.Build().Run();
        }
    }
}
