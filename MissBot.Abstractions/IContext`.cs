
namespace MissBot.Abstractions
{
    public interface IContext<TScope> : IContext, IHandleContext
    {
        TScope Data { get; set; }
        IResponseChannel Create();
    }
}
