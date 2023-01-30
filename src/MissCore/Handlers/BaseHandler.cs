using MissCore.Abstractions;

namespace MissCore.Handlers
{

    public abstract class BaseHandler<TData> : BaseHandleComponent
    {
        public abstract TData GetDataForHandle();
        protected override Task HandleAsync(IHandleContext context)
            => StartHandleAsync(GetDataForHandle(), context);
        public abstract Task StartHandleAsync(TData data, IHandleContext context);
    }
}
