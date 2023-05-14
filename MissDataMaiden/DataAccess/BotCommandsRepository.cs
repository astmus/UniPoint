using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Entities.Common;

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

        public IEnumerable<BotCommand> GetAll()
        {
            GetAllAsync().RunSynchronously();
            return commands;
        }

        public IEnumerable<TEntityType> GetAll<TEntityType>() where TEntityType : BotCommand
        {
            if (commands == null)
                GetAllAsync<TEntityType>().Wait();
            return commands.Where(c => c is TEntityType).Cast<TEntityType>().ToList(); ;
        }

        public async Task<IEnumerable<BotCommand>> GetAllAsync()
        {
            commands = await Context.HandleRequestAsync<Unit<BotCommand>.Collection>(SqlUnit.Entities<BotCommand>(c => new[] { nameof(c.Command), nameof(c.Description) }));
            return commands;
        }

        public async Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType : BotCommand
        {

            var result = await Context.HandleRequestAsync<Unit<TEntityType>.Collection>(SqlUnit.Entities<TEntityType>());

            return result;
        }

        public async Task<TEntityType> GetAsync<TEntityType>() where TEntityType : BotCommand
        {
            var sql = SqlUnit.Command<TEntityType>();
            var cmd = await Context.HandleRequestAsync<TEntityType>(sql);
            return cmd;
        }

        public TCommand GetByName<TCommand>(string name) where TCommand : BotCommand
        {
            throw new NotImplementedException();
        }
    }
}

