using MissBot.Abstractions;
using Newtonsoft.Json.Converters;

namespace BotService.Internal
{
	internal class BotConverter<TBot> : CustomCreationConverter<TBot> where TBot : BaseBot
	{
		private readonly IServiceProvider sp;

		public BotConverter(IServiceProvider sp)
		{
			this.sp = sp;
		}
		public override TBot Create(Type objectType)
		{
			return ActivatorUtilities.GetServiceOrCreateInstance<TBot>(sp);
		}
	}
}
