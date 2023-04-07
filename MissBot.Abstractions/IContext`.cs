
namespace MissBot.Abstractions
{
    public interface IContext<TScope> : IContext, IHandleContext
    {
        TScope Scope { get; set; }
    }
}
