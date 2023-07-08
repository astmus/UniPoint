using Microsoft.Extensions.DependencyInjection;
using MissBot.Entities.Abstractions;
using MissCore.Actions;
using MissCore.Internal;
using MissCore.Presentation.Convert;

namespace BotService
{
	public static class ServicesCollectionExtensions
	{
		public static IServiceCollection RegisterTypes(this IServiceCollection services)
		{
			services.AddTransient<IUnitAction, UnitAction>();
			return services;
		}

		public static IServiceCollection AddConverters(this IServiceCollection services)
		{
			services.AddSingleton<JsonConverter, UnitActionConverter>()
			.AddScoped(typeof(UnitSerializeConverter<>))
			.AddScoped(typeof(ActionSerializeConverter<>))
			.AddScoped(typeof(CombinedUnitSerializeConverter<>));
			return services;
		}
	}

}

