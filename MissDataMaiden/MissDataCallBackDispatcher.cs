using BotService;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Query;
using MissCore;
using MissCore.Data.Collections;
using MissCore.Handlers;
using MissDataMaiden.Entities;

namespace MissDataMaiden
{
    internal class MissDataCallBackDispatcher : CallbackQueryHandler
    {
        public MissDataCallBackDispatcher(IResponseNotification notifier) : base(notifier)
        { }
       // IResponse<CallbackQuery> response;
        protected override async Task HandleAsync(string command, string unit, string id, CallbackQuery query, CancellationToken cancel = default)
        {
            //this.response = response;
            var botUnit = Context.Set(await Context.Bot.GetActionAsync<DataBase>(command));

            await notifier.Complete().ConfigFalse();

            botUnit.UnitIdentifier ??= Id<DataBase>.Value.With(id);
            var handler = Context.GetBotService<InputParametersHandler>();
            await handler.HandleAsync(ParametersEntered, botUnit, Context, cancel);

            Context.SetNextHandler(handler.AsyncDelegate);
        }

        async Task ParametersEntered(FormattableUnitBase result, IHandleContext context)
        {
            //await response.CompleteInput(result.ToString()).Commit();

            var repository = Context.GetBotService<IJsonRepository>();
        
            var unitRes = await repository.RawAsync<GenericUnit>(result.Format, default, result.ToArray());
            //if (unitRes != null)
            //    await response.CompleteInput(unitRes.Content.FirstOrDefault()?.Format("TB")).Commit();
            //else
            //    await response.CompleteInput("Not found").Commit();
            Context.IsHandled = true;
        }
    }
}
