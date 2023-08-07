using MissBot.Abstractions.Bot;

namespace MissBot.Abstractions.Configuration
{
	public interface IBotContextBuilder
	{
		IBotContextBuilder ReceiveCallBacks();
		IBotContextBuilder ReceiveInlineQueries();
		IBotContextBuilder ReceiveInlineResult();
		IBotContextBuilder TrackMessgeChanges();
		IBotContextBuilder AddResponseUnit<TUnit>() where TUnit : BaseBotUnit;
	}
}
