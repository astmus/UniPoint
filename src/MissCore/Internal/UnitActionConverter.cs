using Microsoft.Extensions.DependencyInjection;
using MissBot.Entities.Abstractions;
using Newtonsoft.Json.Converters;

namespace MissCore.Internal
{
	internal class UnitActionConverter : CustomCreationConverter<IUnitAction>
	{
		private readonly IServiceProvider provider;

		public UnitActionConverter(IServiceProvider sp)
			=> provider = sp;

		public override IUnitAction Create(Type objectType)
			=> (IUnitAction)ActivatorUtilities.GetServiceOrCreateInstance(provider, objectType);
	}
}
