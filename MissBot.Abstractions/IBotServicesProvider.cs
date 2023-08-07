
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.Bot;

namespace MissBot.Abstractions
{
	public interface IBotServicesProvider : IServiceProvider
	{
		IBotClient Client { get; }
		IInteraction<TData> InteractionOf<TData>() where TData : class;
		IResponseError ResponseError();
		IEnumerable<object> GetServices(Type serviceType);
		T Activate<T>() where T : class;
		T GetService<T>();
		T GetRequiredService<T>();
	}
}
