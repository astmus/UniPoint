using MissBot.Abstractions;

namespace MissCore.Handlers
{
    public abstract class BaseHandleComponent : IAsyncHandleComponent
    {  
        public abstract Task HandleAsync(IHandleContext context, AsyncHandler next, CancellationToken cancel = default); 
    }
}
