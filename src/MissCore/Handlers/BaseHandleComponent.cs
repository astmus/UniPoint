using MissBot.Abstractions;
using MissCore.Abstractions;

namespace MissCore.Handlers
{
    public abstract class BaseHandleComponent : IAsyncHandler
    {
        protected internal IHandleContext Context { get; protected set; }
        public async Task ExecuteAsync(IHandleContext context, HandleDelegate next)
        {
            if (ItCanBeHandled(context))
                await HandleAsync(Context = context);
            else
                await next(context);
        }
        public abstract bool ItCanBeHandled(IHandleContext context);
        protected abstract Task HandleAsync(IHandleContext context);
    }
}
