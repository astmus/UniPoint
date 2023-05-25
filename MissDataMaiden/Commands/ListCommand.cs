using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore.Bot;

namespace MissDataMaiden
{
    public record List : BotUnitCommand
    {
    }

    public class ListCommandHadler : BotCommandHandler<List>
    {
        public ListCommandHadler(IRepository<BotCommand> repository)
            => this.repository = repository;
            
        private readonly IRepository<BotCommand> repository;

        public override Task HandleCommandAsync(List command,  CancellationToken cancel = default)
        {
            Console.WriteLine(command);
            Context.IsHandled = true;
            return Task.CompletedTask;
        }
    }
}
