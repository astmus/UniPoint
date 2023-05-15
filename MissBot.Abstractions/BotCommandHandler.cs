using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions
{
    public abstract class BotCommandHandler<TCommand> : IAsyncHandler<TCommand> where TCommand : BotCommand
    {         
        public AsyncHandler AsyncHandler { get; }  
        public virtual Task BeforeComamandHandle(TCommand data, IHandleContext context)
                => Task.CompletedTask;
        public virtual Task AfterComamandHandle(TCommand data, IHandleContext context)
            => Task.CompletedTask;
        public virtual Task OnComamandFailed(IHandleContext context, Exception error)
            => Task.CompletedTask;

        public async Task HandleAsync(TCommand data, IHandleContext context, CancellationToken cancel = default)
        {
            try
            {            
                await BeforeComamandHandle(data, context).ConfigureAwait(false);
                await HandleCommandAsync(data, context);
                await AfterComamandHandle(data, context).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                await OnComamandFailed(context, error);
            }
        }

        public abstract Task HandleCommandAsync(TCommand command, IHandleContext context, CancellationToken cancel = default);
    }
}
