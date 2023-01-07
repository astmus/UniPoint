using MissCore.Abstractions;
using MissCore.Handlers;

namespace MissBot.Handlers
{

    public abstract class BaseHandler<TData> : BaseHandleComponent
    {
        public abstract TData GetDataForHandle();
        protected override Task HandleAsync(IHandleContext context)
            => StartHandleAsync(GetDataForHandle(), context);
        public abstract Task StartHandleAsync(TData data, IHandleContext context);
    }
}
