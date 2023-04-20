namespace MissBot.Abstractions
{
   public abstract class BotCommandHandler<TCommand> : IAsyncHandler<TCommand> where TCommand : MissBot.Abstractions.Entities.BotCommand
    {

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
                await HandleCommandAsync(context.Data, context);
                await AfterComamandHandle(context).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                await OnComamandFailed(context, error);
            }
        }

        public abstract Task HandleCommandAsync(TCommand command, IContext<TCommand> context);
    }
}
