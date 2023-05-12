using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess;
using MissBot.DataAccess.Sql;

namespace MissDataMaiden.DataAccess
{
    public class BotCommandsRepository : BotRepository, IBotCommandsRepository
    {
        
        IEnumerable<BotCommand> commands;

        public BotCommandsRepository(IConfiguration configuration, BotDataContext context) : base(configuration, context)
        {
        }

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
            commands = await HandleCommandAsync<Unit<BotCommand>.Collection>(MissBot.Abstractions.DataAccess.Unit.Entities<BotCommand>());
            return commands;
        }

        public async Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType : BotCommand
        {
            
            var result = await HandleCommandAsync<Unit<TEntityType>.Collection>(MissBot.Abstractions.DataAccess.Unit.Entities<TEntityType>());
           
            return result;
        }

        public async Task<TEntityType> GetAsync<TEntityType>() where TEntityType : BotCommand
        {            
            var sql = MissBot.Abstractions.DataAccess.Unit.Command<TEntityType>();
            var cmd = await HandleCommandAsync<TEntityType>(sql);
            return cmd;
        }

        public TCommand GetByName<TCommand>(string name) where TCommand : BotCommand
        {
            throw new NotImplementedException();
        }
    }
}

