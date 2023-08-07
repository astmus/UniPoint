using BotService.Configuration;
using BotService.Connection;
using BotService.Internal;
using LinqToDB;
using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.DataAccess.Async;
using MissCore;
using MissCore.Bot;
using MissCore.Data;
using MissCore.Data.Context;
using MissCore.Extensions;
using MissCore.Handlers;
using MissCore.Response;
using MissCore.Storage;
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
																					//.AddSingleton<IBot,TBot>()
																					.AddSingleton<IBotContext>(sp => sp.GetRequiredService<IBotContext<TBot>>())
																					.AddSingleton<IBotContext<TBot>, BotContext<TBot>>()
																					.AddSingleton<IBotBuilder<TBot>>(sp => botBuilder)
																					.AddSingleton<BaseBot.Configurator, TConfig>()
																					.AddSingleton<JsonConverter, BotConverter<TBot>>()
																					.AddScoped<IResponseError, ErrorResponse>()
																					.AddScoped<IContext<Update<TBot>>, Context<Update<TBot>>>()
																					.AddScoped<IAsyncUpdateHandler<Update<TBot>>, BotUpdateHandler<TBot>>()
																					.AddScoped<IBotUpdatesDispatcher<Update<TBot>>, AsyncBotUpdatesDispatcher<Update<TBot>>>()
																					.AddScoped<IBotUpdatesReceiver<Update<TBot>>, AsyncBotUpdatesReceiver<Update<TBot>>>()
																					.AddScoped<IBotClient>(sp => sp.GetRequiredService<IBotClient<TBot>>())
																					.AddScoped(typeof(IResponse<>), typeof(Response<>))
																					 .AddScoped(sp
																						=> sp.GetRequiredService<IContext<Update<TBot>>>() as IHandleContext)
																					.AddConverters()
																					.RegisterTypes()
																					.AddHttpClient<IBotClient<TBot>, BotConnectionClient<TBot>>(typeof(TBot).Name));

			ConfigureServices((ctx, services) =>
			{
				services.AddOptions<BotContextOptions>().Bind(ctx.Configuration.GetSection(BotContextOptions.ContextOptions));

				services.AddLinqToDBContext<IBotDataContext, BotDataContext>((provider, options) =>
							options.UseSqlServer(ctx.Configuration.GetConnectionString("Default"))
							.UseDefaultLogging(provider));

				services.AddLinqToDBContext<IUserDataContext, UserDataContext>((provider, options) =>
							options.UseSQLite(ctx.Configuration.GetConnectionString("UserConnectionString"))
							.UseDefaultLogging(provider));


				//.AddOptions<DataOptions<BotDataContext>>().Bind(ctx.Configuration.GetSection("UserContextOptions"));
				//services.AddOptions<DataOptions<UserDataContext>>().Configure(opt
				//		=> ctx.Configuration.GetValue<DataOptions<UserDataContext>>("UserContextOptions"));//.Bind(ctx.Configuration.GetSection("UserContextOptions"));
				//																						   //.AddTransient<DataOptions<BotDataContext>>()
				//services.AddOptions<DataOptions>().Bind(ctx.Configuration.GetSection("UserContextOptions"));

			});

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
				//services
				//.AddApplicationServices()
				//.AddInfrastructureServices(host.Configuration);
				services.AddSingleton<IHandleContextFactory, HandleContextFactory>();
				services.AddHttpClient<IBotConnection, BotConnection>();

				services.AddScoped(sp
					=> sp.GetRequiredService<IBotConnectionOptionsBuilder>().Build());
				services.AddScoped<IBotConnectionOptionsBuilder, BotOptionsBuilder>();

				services.AddScoped<IBotOptionsBuilder>(sp
					=> sp.GetRequiredService<IBotConnectionOptionsBuilder>());
				/*services.AddHttpContextAccessor();
				services.AddHealthChecks()
					.AddDbContextCheck<ApplicationDbContext>();*/

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
