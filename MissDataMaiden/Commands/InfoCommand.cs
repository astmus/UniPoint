using MissBot.Abstractions;
using MissBot.Abstractions.DataContext;
using MissBot.Abstractions.Entities;
using MissCore;
using MissCore.Bot;
using MissCore.Collections;
using MissDataMaiden.Queries;

namespace MissDataMaiden.Commands
{
    public record Info : BotUnitCommand
    {
        public override string Command => nameof(Info);
    }

    public record InfoUnit(string s) : Unit<Info>
    {
        public override string Entity { get; }
        public override IMetaData Meta { get => base.Meta; set => base.Meta = value; }
    }

    internal class InfoCommandHadler : BotCommandHandler<Info>
    {
        SqlRaw<InfoUnit>.Query CurrentRequest { get; set; }
        public IJsonRepository repository { get; }
        string connectionString;
        private const string CFG_KEY_COMMAND = nameof(IBotCommand);
        private const string CFG_KEY_DESCRIPTION = nameof(IBotCommand.Description);
        //private const string CFG_KEY_PARAMS = nameof(IBotCommand.Params);
        public InfoCommandHadler(IJsonRepository jsonRepository)
        {
            repository = jsonRepository;
        }

        public async override Task HandleCommandAsync(Info command, CancellationToken cancel = default)
        {
            var response = Context.BotServices.Response<Info>();
            var cmd = Context.Bot.GetCommand<Info>();
            var r = new UnitRequest<Info>(cmd.Payload);

            var metaCollection = await repository.HandleQueryAsync(r);

            foreach (var item in metaCollection.EnumarateAs<Unit<Info>>())
            {
                response.Write(item);
            }

            await response.Commit(default);
        }       
    }
}
