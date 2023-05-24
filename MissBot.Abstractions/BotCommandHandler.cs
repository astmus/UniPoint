using MissBot.Abstractions.Entities;

namespace MissBot.Abstractions
{
    public abstract class BotCommandHandler<TCommand> :  BaseHandler<TCommand> where TCommand : BotCommand
    {
        public virtual Task BeforeComamandHandleAsync(TCommand data)
                => Task.CompletedTask;
        public virtual Task AfterComamandHandleAsync(TCommand data)
            => Task.CompletedTask;
        public virtual Task OnComamandFailed(Exception error)
            => Task.CompletedTask;
        
        public override async Task HandleAsync(TCommand data, CancellationToken cancel = default)
        {
            try
            {            
                await BeforeComamandHandleAsync(data).ConfigureAwait(false);
                await HandleCommandAsync(data, cancel).ConfigureAwait(false);
                await AfterComamandHandleAsync(data).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
                await OnComamandFailed(error);
            }
        }

        public abstract Task HandleCommandAsync(TCommand command, CancellationToken cancel = default);
    }
}
