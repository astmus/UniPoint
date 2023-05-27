using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissBot.Handlers;
using MissCore.Bot;
using MissCore.Collections;
using MissCore.Data.Context;
using MissCore.Handlers;
using MissDataMaiden.Entities;

namespace MissDataMaiden
{
    internal class MissDataCallBackDispatcher : CallbackQueryHandler
    {
        public MissDataCallBackDispatcher(IResponseNotification notifier) : base(notifier)
        { }

        protected override async Task HandleAsync(string command, string unit, string id, IResponse<CallbackQuery> response, CallbackQuery query, CancellationToken cancel = default)
        {
            try
            {
                if (Context.Get<IBotUnitAction<DataBase>>() is not IBotUnitAction<DataBase> botUnit)
                    botUnit = Context.Set(await Context.Bot.GetActionAsync<DataBase>(command));

                botUnit.Identifier ??= Id<DataBase>.Value.With(id);
                var handler = Context.GetBotService<BotUnitActionHandler>();
                Context.SetNextHandler(Context, handler.AsyncDelegate);
                var result = await handler.HandleAsync(botUnit, Context, cancel);
                if (result != null)
                    await Context.BotServices.Response<BotCommand>().CompleteInput(result.ToString()).Commit();

            }
            catch (Exception ex)
            {
                Context.IsHandled = true;
                await notifier.SendTextAsync(ex.Message);
            }
        }
    }
}
