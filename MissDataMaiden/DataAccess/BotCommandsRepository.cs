using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.DataAccess;

namespace MissDataMaiden.DataAccess
{
    public class BotCommandsRepository : SqlRepository, IBotCommandsRepository
    {
        
        IEnumerable<BotCommand> commands;

        public BotCommandsRepository(IConfiguration configuration) : base(configuration)
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
            commands = await HandleSqlQueryAsync<Unit<BotCommand>.Collection>(SQL.Entities<BotCommand>());
            return commands;
        }

        public async Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType : BotCommand
        {
            
            var result = await HandleSqlQueryAsync<Unit<TEntityType>.Collection>(SQL.Entities<TEntityType>());
           
            return result;
        }

        public async Task<TEntityType> GetAsync<TEntityType>() where TEntityType : BotCommand
        {            
            var sql = SQL.CommandUnit<TEntityType>(); // BotContext.Command<TEntityType>(Unit<TEntityType>.Sample).Command;
            var cmd = await HandleSqlQueryAsync<TEntityType>(sql);
            return cmd;
        }

        public TCommand GetByName<TCommand>(string name) where TCommand : BotCommand
        {
            throw new NotImplementedException();
        }
    }
}

