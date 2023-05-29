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
        IResponse<CallbackQuery> response;
        protected override async Task HandleAsync(string command, string unit, string id, IResponse<CallbackQuery> response, CallbackQuery query, CancellationToken cancel = default)
        {
            this.response = response;
            if (Context.Get<IBotUnitAction<DataBase>>() is not IBotUnitAction<DataBase> botUnit)
                botUnit = Context.Set(await Context.Bot.GetActionAsync<DataBase>(command));

            botUnit.Identifier ??= Id<DataBase>.Value.With(id);
            var handler = Context.GetBotService<InputParametersHandler>();
            await handler.HandleAsync(ParametersEntered, botUnit, Context, cancel);           

            Context.SetNextHandler(handler.AsyncDelegate);
        }

        async Task ParametersEntered(FormattableUnitBase result, IHandleContext context)
        {
                await response.CompleteInput(result.ToString()).Commit();

                var repository = Context.GetBotService<IJsonRepository>();
                var unitRes = await repository.RawAsync<DataBase>(result.Format, default, result.ToArray());
                unitRes.Content.FirstOrDefault()?.InitializeMetaData();
                await response.CompleteInput(unitRes.Content.FirstOrDefault()?.Format()).Commit();
                Context.IsHandled = true;            
        }
    }
}
