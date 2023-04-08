using MissBot.Abstractions;


namespace MissCore.Handlers
{

    public abstract class BaseHandler<TData> : IAsyncHandler, ISetupHandler<TData>
    {
        protected TData Data { get; set; }
        public IContext<TData> Context { get; protected set; }
        public ExecuteHandler ExecuteHandler
            => ExecuteAsync;

        public AsyncHandler AsyncHandler
            => HandleAsync;
        public SetupHandler<TData> SetupHandler
            => Setup;

        protected async Task Setup(TData data, IContext<TData> context)
        {
            Data = data;
            Context = context;
            await AsyncHandler(Context);
        }

        protected async Task RunAsync(TData data, IContext<TData> context, AsyncHandler next)
        {
            Data = data;
            Context = context;
            await next(Context);
        }

        public abstract Task ExecuteAsync(CancellationToken cancel = default);

        async Task HandleAsync(IHandleContext context)
        {
            if (context.Get<TData>() is TData data)
                await ExecuteHandler();
            else
                await context.Get<AsyncHandler>()(context).ConfigureAwait(false);
        }
    }
}
