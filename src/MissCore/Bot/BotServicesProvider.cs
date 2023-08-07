using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Bot;
using MissCore.Response;

namespace MissCore.Bot
{
	public class BotServicesProvider : IBotServicesProvider
	{
		IServiceProvider sp;
		public BotServicesProvider(IServiceProvider spr)
			=> sp = spr;

		public IBotClient Client
			=> GetRequiredService<IBotClient>();

		public IResponseError ResponseError()
			=> GetRequiredService<IResponseError>();

		public T GetRequiredService<T>()
			=> sp.GetRequiredService<T>();

		public T GetService<T>()
			=> sp.GetService<T>();

		public IEnumerable<object?> GetServices(Type serviceType)
			=> sp.GetServices(serviceType);

		public object? GetService(Type serviceType)
			=> sp.GetService(serviceType);

		internal IResponse<T> Response<T>() where T : class
			=> GetRequiredService<IResponse<T>>();

		public T Activate<T>() where T : class
			=> ActivatorUtilities.GetServiceOrCreateInstance<T>(sp);

		public IInteraction<TData> InteractionOf<TData>() where TData : class
			=> ActivatorUtilities.GetServiceOrCreateInstance<UnitActionResponse<TData>>(sp);
	}
}
