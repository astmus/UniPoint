using MissBot.Abstractions;
using MissCore.Entities;

namespace MissCore.Handlers
{

    public abstract class ContextHandler<T> : IContextHandler<T> where T:IUpdateInfo
    {
        public async Task ExecuteAsync(IHandleContext context, AsyncHandler next)
        {
            if (context.GetAny<Update>() is IUpdate<T> update)
            {
                SetupContext(context, update.Data);
            }
            await next(context).ConfigureAwait(false);
        }

        public abstract void SetupContext(IContext context, T update);   
    }

}
