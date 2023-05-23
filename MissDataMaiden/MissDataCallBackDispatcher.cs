using MediatR;
using MissBot.Abstractions;
using MissBot.Entities.Query;
using MissBot.Handlers;
using MissCore.Collections;
using MissDataMaiden.Entities;

namespace MissDataMaiden
{
    internal class MissDataCallBackDispatcher : CallbackQueryHandler
    {
        public MissDataCallBackDispatcher(IResponseNotification notifier) : base(notifier)
        { }

        protected override async Task HandleAsync(string command, string[] args, IResponse<CallbackQuery> response, CallbackQuery query, CancellationToken cancel = default)
        {
            try
            {
                var unit = new Unit<DataBase>();
                unit.InitializaMetaData();
                unit.Meta.SetItem(nameof(command), command);                
                unit.Meta.SetItem("Id", args[1]);
                await notifier.SendTextAsync("Wait...");
                await Task.Delay(1000);
                //response.Write(unit);
                await response.Commit(cancel);
                Context.IsHandled = true;
            }
            catch (Exception ex)
            {
                Context.IsHandled = true;
                await notifier.SendTextAsync(ex.Message);
            }
        }
    }
}
