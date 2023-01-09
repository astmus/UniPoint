using MissCore.DataAccess.Async;
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

    public interface IBotUpdatesDispatcher<TUpdate> where TUpdate :  IUpdateInfo
    {
        void Initialize(CancellationToken cancel = default);
        public void PushUpdate(TUpdate update);

    }
}
