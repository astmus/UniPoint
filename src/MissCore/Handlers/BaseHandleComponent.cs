using MissBot.Abstractions;

namespace MissCore.Handlers
{
    public abstract class BaseHandleComponent : IAsyncHandler
    {
        public AsyncHandler AsyncHandler
            => HandleAsync;

        public abstract Task ExecuteAsync(IHandleContext context);


        protected async virtual Task HandleAsync(IHandleContext context)
        {
            AsyncHandler next = context.Get<AsyncHandler>();
            await ExecuteAsync(context);
            await next(context).ConfigureAwait(false);
        }
    }
}
