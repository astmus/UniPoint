using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities;
using MissBot.Identity;
using MissCore.Bot;
using MissCore.Data.Collections;

namespace MissCore.Data.Context
{
	public class Context<TScope> : Context, IContext<TScope>, IHandleContext
	{
		protected IHandleContext Root { get; set; }
		IBotServicesProvider botServices;
		public bool? IsHandled { get; set; }
		AsyncHandler currentHandler;
		public Context(IBotServicesProvider botServiceProvider)
		{
			botServices = botServiceProvider;
			Root = this;
		}
		public IBotServicesProvider BotServices
			=> botServices ?? Root.BotServices;

		public IContext<TScope> SetData(TScope data)
		{
			if (Map is DataMap map)
				map.JoinData(data);
			else
				Map = DataMap.Parse(data);

			Root.Set(data);
			return this;
		}

		public T GetBotService<T>() where T : class
			=> BotServices.GetService<T>();
		public THandler GetAsyncHandler<THandler>() where THandler : class, IAsyncHandler
			=> BotServices.GetService<THandler>();

		public IAsyncHandler<T> GetAsyncHandlerOf<T>()
			=> BotServices.GetService<IAsyncHandler<T>>();

		public bool Contains<T>(Id<T> identifier)
			=> ContainsKey(identifier.Value) || Map.Contains(identifier.Value);

		public IHandleContext SetNextHandler(AsyncHandler handler)
		{
			Set(handler);
			return this;
		}

		public async Task GetNextHandler(AsyncHandler defHandler = default)
		{
			currentHandler = Get<AsyncHandler>(defHandler);
			await currentHandler(this);
		}

		public AsyncHandler CurrentHandler
			=> currentHandler;

		public IBotContext Bot
			=> BotServices.GetRequiredService<IBotContext>();

		public IResponse<T> Response<T>() where T : class
			=> BotServices.GetService<IResponse<T>>();
	}
}
