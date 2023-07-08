using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Handlers;

using MissCore;
using MissCore.Bot;
using MissCore.Data;
using MissCore.Data.Collections;
using MissCore.DataAccess;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace MissDataMaiden.Commands
{
    [JsonObject]
    public record Disk : BotUnitCommand
    {
    }


    public record DiskCommand : BotCommand<Disk>
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

            IUnitRequest request = Context.Bot.CreateUnitRequest(command);


            var metaCollection = await repository.HandleQueryJsonAsync<GenericUnit>(request);
            var items = metaCollection.Enumarate<Unit<Disk>>();
            ///response.Write(metaCollection.EnumarateUnits<Unit<Disk>>());
            foreach (var item in items)
            {
                response.WriteUnit(item);
            }

            await response.Commit(default);
        }
    }
}
