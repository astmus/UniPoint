using MediatR;
using MissBot.Abstractions;

namespace MissCore.Handlers
{
    public class MediatorMiddleware : IAsyncHandler
    {
        IMediator mm;

        public MediatorMiddleware(IMediator mediator)
            => mm = mediator;

        public ExecuteHandler ExecuteHandler { get; }
        public AsyncHandler AsyncDelegate { get; }

        public Task ExecuteAsync(IHandleContext context, AsyncHandler next)
        {
            context.Set(mm);
            return next(context);
        }
    }
}
