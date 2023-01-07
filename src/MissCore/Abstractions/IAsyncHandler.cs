using MissCore.Handlers;

namespace MissCore.Abstractions
{
    public interface IAsyncHandler
    {
        Task ExecuteAsync(IHandleContext context, HandleDelegate next);
    }
}
