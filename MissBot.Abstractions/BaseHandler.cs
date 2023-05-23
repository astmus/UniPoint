namespace MissBot.Abstractions
{
    public abstract class BaseHandler<TData> : IAsyncHandler<TData>
    {
        public AsyncHandler AsyncHandler
            => HandleAsync;
        public IHandleContext Context { get; set; }
        async Task HandleAsync(IHandleContext context)
        {
            if (context.Get<TData>() is TData data)
            {
                Context = context;
                await HandleAsync(data);
            }
            if (context.IsHandled.HasValue)
                return;
         
            await context.CurrentHandler(context).ConfigureAwait(false);
        }

        public abstract Task HandleAsync(TData data, CancellationToken cancel = default);        
    }
}
