using MissCore.Handlers;

namespace MissCore.Abstractions
{
    public interface IAsyncHandler
    {
        Task ExecuteAsync(IHandleContext context, HandleDelegate next);
    }
    public interface IAsyncHandler<T>
    {
        Task HandleAsync(IContext<T> context, T data);
    }
}
