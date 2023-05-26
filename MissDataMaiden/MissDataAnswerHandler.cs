using BotService.Common;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities;
using MissBot.Entities.Results;
using MissBot.Handlers;
using MissCore;
using MissCore.Bot;
using MissCore.Collections;
using MissCore.Data.Context;
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
        
        public override async Task HandleResultAsync(IResponse<ChosenInlineResult> response, Message message, ChosenInlineResult result, CancellationToken cancel = default)
        {         
            var info = Context.Bot.Get<DataBaseInfo>();
            var rawRequest = info.Format(6/*result.Id*/);
            Info dbInfo = await repository.HandleRawAsync<Info>(rawRequest, cancel);            
            response.Write(dbInfo);
            //foreach (var item in items.SupplyTo<DataBase>())
            //{
            //    response.Write(item);
            //    //cmdinfo.Unit = item;                
            //}
            await response.Commit(cancel);

        }
    }
}

