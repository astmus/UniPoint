using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities;
using MissBot.Entities.Results;
using MissCore;
using MissCore.Collections;
using MissCore.Data;
using MissCore.Handlers;
using MissDataMaiden.Entities;


namespace MissDataMaiden
{
    internal class MissDataAnswerHandler : InlineAnswerHandler
    {
        IJsonRepository repository;
        public MissDataAnswerHandler(IJsonRepository jsonRepository)
        {
            repository = jsonRepository;
        }
        
        public override async Task HandleResultAsync(Message message, ChosenInlineResult result, CancellationToken cancel = default)
        {         
            var unit = Context.Bot.Get<DataBaseInfo>();
            unit.Id = result.Id;

            var request = BotUnitRequest.Create(unit);
            var r = Context.BotServices.Response<DataBase>();
            var dbInfo = await repository.HandleScalarAsync<Info>(request, cancel);
            
            
            var bunit = await Context.Bot.GetBotUnitAsync<DataBase>();
            bunit.SetUnitActions(dbInfo);
            

            r.Write(dbInfo);
        
            await r.Commit(cancel);

        }
    }
}

