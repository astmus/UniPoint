namespace MissBot.Abstractions
{
    //public class BotCommandHandler : IAsyncBotCommandHandler
    //{

    //    public Task HandleAsync(IHandleContext context, IBotCommandData command)
    //    => HandleCommandAsync(context);

    //    public  async Task HandleCommandAsync<TCommand>(IContext<TCommand> context) where TCommand:class,IBotCommandData
    //    {
    //        var ctx = context.BotServices.GetService<IContext<TCommand>>();
    //        ctx.Response.SetContext(context);
    //        var handler = context.BotServices.GetRequiredService<IAsyncHandler<TCommand>>();
    //        ctx.Data = Activator.CreateInstance<TCommand>();

    //        //await BeforeComamandHandle(ctx).ConfigureAwait(false);
    //        try
    //        {

    //            await handler.HandleAsync(ctx);
    //            //await AfterComamandHandle(ctx).ConfigureAwait(false);
    //        }
    //        catch (Exception error)
    //        {
    //            Console.WriteLine(error.Message);
    //           // await OnComamandFailed(ctx, error);
    //        }
    //    }

    //    Task IAsyncBotCommandHandler.HandleAsync<TCommand>(IHandleContext context)
    //    {
    //        throw new NotImplementedException();
    //    }
    //}

    public abstract class BotCommandHandler<TCommand> : IAsyncHandler<TCommand> where TCommand : IBotCommand
    {
        public abstract TCommand Command { get; }
        public ExecuteHandler ExecuteHandler { get; }
        public AsyncHandler AsyncHandler { get; }

        public AsyncGenericHandler<TCommand> GenericHandler
            => HandleAsync;

        public virtual Task BeforeComamandHandle(IContext<TCommand> context)
                => Task.CompletedTask;
        public virtual Task AfterComamandHandle(IContext<TCommand> context)
            => Task.CompletedTask;
        public virtual Task OnComamandFailed(IContext<TCommand> context, Exception error)
            => Task.CompletedTask;

        public async Task HandleAsync(IContext<TCommand> context)
        {
            try
            {
                await BeforeComamandHandle(context).ConfigureAwait(false);
                await RunAsync(context.Data, context);
                await AfterComamandHandle(context).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                await OnComamandFailed(context, error);
            }
        }

        public abstract Task RunAsync(TCommand command, IContext<TCommand> context);
    }
}
