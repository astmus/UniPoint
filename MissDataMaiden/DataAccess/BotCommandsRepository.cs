using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;


namespace MissDataMaiden.DataAccess
{
    public class BotCommandsRepository : IBotCommandsRepository
    {

        IEnumerable<BotCommand> commands;

        public BotCommandsRepository(IBotContext context)
        {
            Context = context;
        }

        public IBotContext Context { get; }
        public IList<BotCommand> Commands { get; protected set; }

        public void AddCommand<TCommand>(TCommand command) where TCommand : BotCommand
        {
            Context.Commands.Add(command);
        }

        public Task<bool> Commit(CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<BotCommand> GetAll()
        {            
            return commands;
        }

        public IEnumerable<TEntityType> GetAll<TEntityType>() where TEntityType : BotCommand
        {            
            return commands.Where(c => c is TEntityType).Cast<TEntityType>().ToList();
        }

        public Task<IEnumerable<BotCommand>> GetAllAsync()
        {
            commands = Context.Commands;
            return Task.FromResult(commands);
        }

        public  Task<TCommand> GetAsync<TCommand>() where TCommand : BotCommand
        {
            return Task.FromResult(Context.GetCommand<TCommand>());
        }
    }
}

