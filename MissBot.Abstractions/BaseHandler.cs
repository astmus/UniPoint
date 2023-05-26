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
                //try
                //{                    
                    Context = context;
                    await HandleAsync(data);
                //}
                //catch (Exception error)
                //{
                //    var response = context.BotServices.ErrorResponse();
                //    response.Write(error);
                //    await response.Commit();// context.BotServices.Client.MakeRequestAsync(response);
                //    context.IsHandled = true;
                //}
            }

            if (context.IsHandled.HasValue)
                return;
         
            await context.MoveToNextHandler();
        }

        public void SetContext(IHandleContext context)
            => Context = context;

        public abstract Task HandleAsync(TData data, CancellationToken cancel = default);        
    }
}
