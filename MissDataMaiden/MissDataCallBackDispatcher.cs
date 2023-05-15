using MediatR;
using MissBot.Abstractions;
using MissBot.Entities.Query;
using MissBot.Handlers;

namespace MissDataMaiden
{
    internal class MissDataCallBackDispatcher : CallbackQueryHandler
    {
        IMediator mm;

        public MissDataCallBackDispatcher(IMediator mediator, IResponseNotification notifier) : base(notifier)
        { }



        //protected override Task HandleAsync(IHandleContext context, string command, string[] args) => command switch
        //{
        //    nameof(DBInfo) => HandleAsync<DBInfo>(context, args),
        //    nameof(DBDelete) => HandleAsync<DBDelete>(context, args),
        //    nameof(DBRestore) => HandleAsync<DBRestore>(context, args),
        //    _ => context.Take<AsyncHandler>()(context)
        //};


        protected  override Task HandleAsync(IHandleContext context, (string command, string[] args) data, CallbackQuery query)
            => Task.CompletedTask;
        public override async Task HandleAsync(CallbackQuery data, IHandleContext context, CancellationToken cancel = default)
        {
            try
            {

            }
            catch (Exception ex)
            {
                await notifier.SendTextAsync(ex.Message);
            }
        }
    }
}
