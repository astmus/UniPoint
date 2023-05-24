namespace MissBot.Abstractions
{
    public abstract class BaseHandler<TData> : IAsyncHandler<TData>
    {
        public virtual AsyncHandler AsyncDelegate
            => HandleAsync;

        public IHandleContext Context { get; private set; }
        async Task HandleAsync(IHandleContext context)
        {
            if (context.Get<TData>() is TData data)
            {
                try
                {
                    Context = context;
                    await HandleAsync(data);
                }
                catch (Exception error)
                {
                    var command = context.BotServices.ErrorResponse();
                    command.Write(error);
                    await context.BotServices.Client.MakeRequestAsync(command);
                }
            }

            if (context.IsHandled.HasValue)
                return;
         
            await context.CurrentHandler(context).ConfigureAwait(false);
        }

        public void SetContext(IHandleContext context)
            => Context = context;

        public abstract Task HandleAsync(TData data, CancellationToken cancel = default);        
    }
}
