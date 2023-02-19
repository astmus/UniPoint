using MissBot.Abstractions;


namespace MissCore.Handlers
{

    public abstract class BaseHandler<TData> : IAsyncHandler
    {
        protected abstract IHandleContext Context { get; set; }       
        
        public abstract Task StartHandleAsync(TData data, IHandleContext context);

        public Task HandleAsync(IContext<TData> context, TData data)
        => StartHandleAsync(data, Context);

        public async Task ExecuteAsync(IHandleContext context, HandleDelegate next)
        {
            if (context.Update is TData data)
                await StartHandleAsync(data, context);
            await next(context);
        }


    }
}
