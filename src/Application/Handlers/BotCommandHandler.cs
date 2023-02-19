using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;

namespace MissBot.Handlers
{
    public abstract class BotCommandHandler : IAsyncBotCommansHandler
    {
        public abstract Task HandleCommandAsync(IHandleContext context, IBotCommandData command);                
    }
    public abstract class BotCommandHandler<TCommand> : BotCommandHandler, IAsyncHandler<TCommand> where TCommand : class, IBotCommandData
    {
        public virtual TCommand Model {get;set;}
        public virtual Task BeforeComamandHandle(IContext<TCommand> context)
                => Task.CompletedTask;
        public virtual Task AfterComamandHandle(IContext<TCommand> context)
            => Task.CompletedTask;
        public virtual Task OnComamandFailed(IContext<TCommand> context, Exception error)
            => Task.CompletedTask;
        public abstract Task HandleAsync(IContext<TCommand> context, TCommand data);
        public override sealed async Task HandleCommandAsync(IHandleContext context, IBotCommandData command)
        {
            var ctx = context.BotServices.GetService<IContext<TCommand>>();
            ctx.Response.SetContext(context);
            ctx.Set(context.Update as Update);
            await BeforeComamandHandle(ctx).ConfigureAwait(false);
            try
            {
                await HandleAsync(ctx, command as TCommand);
                await AfterComamandHandle(ctx).ConfigureAwait(false);
            }
            catch (Exception error)
            {
                await OnComamandFailed(ctx, error);
            }
            ctx.Data = command as TCommand;
        }

    }
}
