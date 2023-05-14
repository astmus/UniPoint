using MediatR;
using MissBot.Abstractions;
using MissBot.Entities;
using MissBot.Entities.Common;
using MissBot.Handlers;
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
            _ => context.Take<AsyncHandler>()(context)
        };

        public async Task HandleAsync<TAction>(IHandleContext context, string[] args) where TAction : EntityAction<DataBase>
        {

            var ctx = context.CreateDataContext<TAction>();
            ctx.Data = Unit<TAction>.Sample with {  };

            var handler = context.GetAsyncHandler<TAction>();
            try
            {
                await handler.HandleAsync(ctx.Data, context);
            }
            catch (Exception ex)
            {
                await notifier.SendTextAsync(ex.Message);
            }
        }
    }
}
