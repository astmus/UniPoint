using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;

namespace MissBot.Handlers
{
    public class BotCommandHandler : IAsyncBotCommandHandler
    {

        public Task HandleAsync(IHandleContext context, IBotCommandData command)
        => HandleCommandAsync(null, command);

        public  async Task HandleCommandAsync<TCommand>(IContext<TCommand> context) where TCommand:class,IBotCommandData
        {
            var ctx = context.BotServices.GetService<IContext<TCommand>>();
            ctx.Response.SetContext(context);
            var handler = context.BotServices.GetRequiredService<IAsyncHandler<TCommand>>();
            ctx.Data = Activator.CreateInstance<TCommand>();

            //await BeforeComamandHandle(ctx).ConfigureAwait(false);
            try
            {
            
                await handler.HandleAsync(ctx);
                //await AfterComamandHandle(ctx).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                Console.WriteLine(error.Message);
               // await OnComamandFailed(ctx, error);
            }
        }

        Task IAsyncBotCommandHandler.HandleAsync<TCommand>(IHandleContext context)
        {
            throw new NotImplementedException();
        }
    }

    public abstract class BotCommandHandler<TCommand> :  IAsyncHandler<TCommand> where TCommand : class, IBotCommandData
    {
        public TCommand Command { get; set; } = Activator.CreateInstance<TCommand>();
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

        public abstract Task HandleAsync(IContext<TCommand> context);
        
    }
}
