using BotService.Configuration;
using BotService.Connection;
using BotService.Internal;
using Microsoft.Extensions.DependencyInjection;
using MissBot;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess.Sql;
using MissBot.Entities;
using MissBot.Infrastructure;
using MissBot.Infrastructure.Persistence;
using MissCore.Data.Context;
using MissCore.DataAccess;
using MissCore.DataAccess.Async;
using MissCore.Handlers;
using Newtonsoft.Json;

namespace BotService
{
    public class BotHost : HostBuilder, IBotHost
    {

        //internal BotHost(IHostBuilder builder)
        //    => hostBuilder = builder;

        //List<Action> buildActions = new List<Action>();
        //ILogger<BotHost> log;
        //public BotHost(ILogger<BotHost> logger)
        //{
        //    log = logger;
        //}

        public IBotBuilder<TBot> AddBot<TBot, TConfig>() where TBot : BaseBot where TConfig : BaseBot.Configurator
        {
            var botBuilder = BotBuilder<TBot>.GetInstance(this);
            ConfigureServices((ctx, services) => services
                                                                                    .AddHostedService<BotClient<TBot>>()
                                                                                    .AddScoped<TBot>()
                                                                                    .AddSingleton<IBotContext, BotContext>()
                                                                                    .AddScoped<IAsyncHandler<Update<TBot>>, BotUpdateHandler<TBot>>()
                                                                                    .AddScoped<IBotUpdatesDispatcher<Update<TBot>>, AsyncBotUpdatesDispatcher<Update<TBot>>>()
                                                                                    .AddScoped<IBotUpdatesReceiver<Update<TBot>>, AsyncBotUpdatesReceiver<Update<TBot>>>()
                                                                                    .AddSingleton<IBotBuilder<TBot>>(sp => botBuilder)
                                                                                    .AddSingleton<BaseBot.Configurator, TConfig>()
                                                                                    .AddTransient<JsonConverter, BotConverter<TBot>>()
                                                                                    .AddScoped < IHandleContext, Context<Update < TBot >>>(sp
                                                                                        => sp.GetRequiredService<IContext<Update<TBot>>>() as Context<Update<TBot>>)
                                                                                    .AddScoped<IBotClient>(sp => sp.GetRequiredService<IBotClient<TBot>>())
                                                                                    .AddHttpClient<IBotClient<TBot>, BotConnectionClient<TBot>>(typeof(TBot).Name));
            ConfigureServices((ctx, services) =>
                                                                services.AddOptions<BotContextOptions>().Bind(ctx.Configuration.GetSection(BotContextOptions.ContextOptions)));

            //buildActions.Add(() => BotBuilder<TBot>.Instance.Build());
            //var builder = 
            //builder.Services.AddScoped<IBotClient>(sp => sp.GetRequiredService<IBotClient<TBot>>());   
            //builder.Services.AddHttpClient<IBotClient<TBot>, BotConnectionClient<TBot>>(typeof(TBot).Name);
            hostBuilder = this;
            return botBuilder;
        }

        internal IBotHost hostBuilder;
        internal static IBotHost BotsHost;

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
            return hostBuilder;
        }




        public void Start()
        {
            this.Build().Run();
        }


    }
}
