using MissBot.Abstractions;


namespace MissCore.Handlers
{
    public abstract class BaseHandler<TData> : IAsyncHandler<TData>
    {
        public AsyncHandler AsyncHandler
            => HandleAsync;
        public IHandleContext Context { get; set; }
        async Task HandleAsync(IHandleContext context)
        {
            Context = context;
            if (context.Get<TData>() is TData data)
                await HandleAsync(data, context);
            else
                await context.Handler(context).ConfigureAwait(false);
        }

        public abstract Task HandleAsync(TData data, IHandleContext context, CancellationToken  cancel = default);
    }
}
