using MediatR;

using MissCore.Abstractions;
using MissCore.Handlers;

namespace MissBot.Handlers
{
    public class MediatorMiddleware : IAsyncHandler
    {
        IMediator mm;

        public MediatorMiddleware(IMediator mediator)
            => mm = mediator;

        public Task ExecuteAsync(IHandleContext context, HandleDelegate next)
        {
            context.Data.Set(mm);
            return next(context);
        }
    }
}
