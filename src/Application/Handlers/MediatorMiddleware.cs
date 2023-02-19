using MediatR;
using MissBot.Abstractions;
using MissCore.Abstractions;

namespace MissBot.Handlers
{
    public class MediatorMiddleware : IAsyncHandler
    {
        IMediator mm;

        public MediatorMiddleware(IMediator mediator)
            => mm = mediator;

        public Task ExecuteAsync(IHandleContext context, HandleDelegate next)
        {
            context.ContextData.Set(mm);
            return next(context);
        }
    }
}
