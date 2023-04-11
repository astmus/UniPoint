using MediatR;
using MissBot.Abstractions;
using MissBot.Extensions.Entities;
using MissBot.Handlers;
using MissCore.Entities;
using MissCore.Handlers;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    internal class MissDataCallBackDispatcher : CallbackQueryHandler
    {
        IMediator mm;
        public MissDataCallBackDispatcher(IMediator mediator)
            => mm = mediator;

        protected override Task HandleAsync(IHandleContext context, string command, string[] args) => command switch
        {
            nameof(DBInfo) => HandleAsync<DBInfo>(context, args),
            nameof(DBDelete) => HandleAsync<DBDelete>(context, args),
            nameof(DBRestore) => HandleAsync<DBRestore>(context, args),
            _ => context.Get<AsyncHandler>()(context)
        };

        public async Task HandleAsync<TAction>(IHandleContext context, string[] args) where TAction : InlineAction
        {        

            var ctx = context.CreateDataContext<TAction>();
            ctx.InitContextData(args);

            var handler = context.GetAsyncHandler<TAction>();
            
            await handler.HandleAsync(ctx);
            
            //if (context.GetNextHandler<TAction>() is IAsyncHandler<TAction> next)
            //    await next.HandleAsync(ctx).ConfigureAwait(false);
        }
    }
}
