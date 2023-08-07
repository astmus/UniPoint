
namespace MissBot.Abstractions
{
	public interface IContext<TScope> : IContext
	{
		IContext<TScope> SetData(TScope data);
		bool? IsHandled { get; set; }
	}
}
