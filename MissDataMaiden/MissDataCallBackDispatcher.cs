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

            if (Context.Get<IBotUnitAction<DataBase>>(id) is not IBotUnitAction<DataBase> botUnit)
                botUnit = Context.Set(await Context.Bot.GetActionAsync<DataBase>(command), id);

            botUnit.Identifier ??= Id<DataBase>.Value.With(id);
            var handler = Context.GetBotService<BotUnitActionHandler>();
            var result = await handler.HandleAsync(botUnit, Context, cancel);

            if (result != null)
            {
                await Context.BotServices.Response<BotCommand>().CompleteInput(result.ToString()).Commit();

                var repository = Context.GetBotService<IJsonRepository>();
                var unitRes = await repository.RawAsync<DataBase>(result.Format, cancel, result.ToArray());
                unitRes.Content.FirstOrDefault()?.InitializeMetaData();
                await Context.BotServices.Response<BotCommand>().CompleteInput(unitRes.Content.FirstOrDefault()?.Format()).Commit();
                Context.IsHandled = true;
            }

            Context.SetNextHandler(handler.AsyncDelegate);
        }
    }
}
