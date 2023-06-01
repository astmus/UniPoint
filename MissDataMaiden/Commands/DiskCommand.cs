using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore;
using MissCore.Bot;
using MissCore.Collections;
using MissCore.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissDataMaiden.Commands
{
    [JsonObject]
    public record Disk : BotUnitCommand
    {
              
    }   

    public class DiskCommandHandler : BotCommandHandler<Disk>
    {        
        private readonly IJsonRepository repository;        

        public DiskCommandHandler(IJsonRepository repository)
        {
            this.repository = repository;
        }

        public async override Task HandleCommandAsync(Disk command, CancellationToken cancel = default)
        {
            
            var response = Context.BotServices.Response<Disk>();
            var cmd =  Context.Bot.GetCommand<Disk>();
            var r = new UnitRequest<Disk>(cmd.Payload);

            var metaCollection = await repository.HandleQueryAsync(r);             
            
            foreach (var item in metaCollection.EnumarateAs<Unit<Disk>>())
            {                                
               // response.Write(item);
            }

            await response.Commit(default);
        }


    }
}
