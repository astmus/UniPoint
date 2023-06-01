namespace MissBot.Abstractions.Handlers
{
    public abstract class BaseHandler<TData> : IAsyncHandler<TData>
    {
        public virtual AsyncHandler AsyncDelegate
            => HandleAsync;

        public IHandleContext Context { get; private set; }
        async Task HandleAsync(IHandleContext context)
        {
            if (context.Get<TData>() is not TData data)
            {
                await context.GetNextHandler().ConfigureAwait(false);
                return;
            }

            Context = context;
            await HandleAsync(context.Get<TData>());

            if (context.IsHandled.HasValue)
                return;
        }

        public void SetContext(IHandleContext context)
            => Context = context;

        public abstract Task HandleAsync(TData data, CancellationToken cancel = default);
    }
}
