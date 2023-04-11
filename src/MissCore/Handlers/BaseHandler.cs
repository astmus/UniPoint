using MissBot.Abstractions;


namespace MissCore.Handlers
{
    public abstract class BaseHandler<TData> : IAsyncHandler<TData>
    {
        protected TData Data { get; set; }
        public IContext<TData> Context { get; protected set; }
        public ExecuteHandler ExecuteHandler
            => ExecuteAsync;

        public AsyncHandler AsyncHandler
            => HandleAsync;    

        public AsyncGenericHandler<TData> GenericHandler { get; }        

        public virtual Task ExecuteAsync(CancellationToken cancel = default)
            => Task.CompletedTask;

        async Task HandleAsync(IHandleContext context)
        {
            if (context.Get<TData>() is TData data)
                await HandleAsync(context.CreateDataContext<TData>(data));
            else
                await context.Get<AsyncHandler>()(context).ConfigureAwait(false);
        }

        public abstract Task HandleAsync(IContext<TData> context);
    }
}
