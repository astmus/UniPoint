using MissBot.Abstractions;
using MissCore.Entities;

namespace MissCore.Handlers
{

    public abstract class ContextHandler<T> : IContextHandler<T> where T:IUpdateInfo
    {
        public async Task ExecuteAsync(IHandleContext context, HandleDelegate next)
        {
            if (context.Update is T update)
            {
                SetupContext(context.ContextData, update);
            }
            await next(context).ConfigureAwait(false);
        }

        public abstract void SetupContext(IContext context, T update);

   
    }

}
