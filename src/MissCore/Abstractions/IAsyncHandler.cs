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
    public interface IBotUpdatesDispatcher
    {
        void Initialize(CancellationToken cancel = default);
    }
    public interface IBotUpdatesDispatcher<TUpdate> : IBotUpdatesDispatcher where TUpdate :  IUpdateInfo
    {
        public void PushUpdate(TUpdate update);
    }
}
