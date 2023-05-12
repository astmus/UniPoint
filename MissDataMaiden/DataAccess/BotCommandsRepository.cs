using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;

namespace MissDataMaiden.DataAccess
{
    public class BotCommandsRepository : IBotCommandsRepository
    {
        
        IEnumerable<BotCommand> commands;

        public BotCommandsRepository(IJsonRepository context)
        {
            Context = context;
        }

        public IJsonRepository Context { get; }

        public IEnumerable<BotCommand> GetAll()
        {
            GetAllAsync().RunSynchronously();
            return commands;
        }

        public IEnumerable<TEntityType> GetAll<TEntityType>() where TEntityType : BotCommand
        {
            if (commands == null)
                GetAllAsync<TEntityType>().Wait();
            return commands.Where(c => c is TEntityType).Cast<TEntityType>().ToList();;
        }

        public async Task<IEnumerable<BotCommand>> GetAllAsync()
        {
            commands = await Context.HandleQueryAsync<Unit<BotCommand>.Collection>(SqlUnit.Entities<BotCommand>(c => new[] { nameof(c.Command), nameof(c.Description) }));
            return commands;
        }

        public async Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType : BotCommand
        {
            
            var result = await Context.HandleQueryAsync<Unit<TEntityType>.Collection>(SqlUnit.Entities<TEntityType>());
           
            return result;
        }

        public async Task<TEntityType> GetAsync<TEntityType>() where TEntityType : BotCommand
        {            
            var sql = MissBot.Abstractions.DataAccess.SqlUnit.Command<TEntityType>();
            var cmd = await Context.HandleQueryAsync<TEntityType>(sql);
            return cmd;
        }

        public TCommand GetByName<TCommand>(string name) where TCommand : BotCommand
        {
            throw new NotImplementedException();
        }
    }
}

