using MediatR;
using MissBot.Abstractions;
using MissBot.Attributes;
using MissBot.Extensions.Entities;
using MissBot.Handlers;
using MissCore.Entities;
using MissCore.Handlers;
using MissDataMaiden.Commands;
using MissDataMaiden.Entities;

namespace MissDataMaiden
{
    internal class MissDataCallBackDispatcher : CallbackQueryHandler
    {
        IMediator mm;
 
        public MissDataCallBackDispatcher(IMediator mediator, IResponseNotification notifier) : base(notifier)
        { }

        protected override Task HandleAsync(IHandleContext context, string command, string[] args) => command switch
        {
            nameof(DBInfo) => HandleAsync<DBInfo>(context, args),
            nameof(DBDelete) => HandleAsync<DBDelete>(context, args),
            nameof(DBRestore) => HandleAsync<DBRestore>(context, args),
            _ => context.Get<AsyncHandler>()(context)
        };

        public async Task HandleAsync<TAction>(IHandleContext context, string[] args) where TAction : EntityAction<DataBase>
        {        

            var ctx = context.CreateDataContext<TAction>();
            ctx.Data = Unit<TAction>.Sample with { Id = args[0] };
            
            var handler = context.GetAsyncHandler<TAction>();
            try
            {
                await handler.HandleAsync(ctx);
            }
            catch (Exception ex)
            {
                await notifier.SendTextAsync(ex.Message);
            }
        }
    }
}
