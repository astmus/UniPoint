using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotService;
using MediatR;
using Microsoft.Data.SqlClient;
using MissBot.Abstractions.DataAccess;
using MissDataMaiden.Queries;
using Newtonsoft.Json;
using Telegram.Bot.Types;

namespace MissDataMaiden.DataAccess
{
    
    public class BotCommandsRepository : IRepository<BotCommand>
    {
        private readonly IConfiguration config;
        private readonly IMediator mediator;
        IEnumerable<BotCommand> commands;
        BotCommand lastResult;
        public BotCommandsRepository(IConfiguration config, IMediator mediator)
        {
            this.config = config;
            this.mediator = mediator;
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
            return Enumerable.Empty<TEntityType>().Append(lastResult as TEntityType);
        }

        public async Task<IEnumerable<BotCommand>> GetAllAsync()
        {
            var query = SqlQuery<BotCommand>.Instance with { sql = "SELECT * FROM ##BotCommands FOR JSON AUTO", connectionString = config.GetConnectionString("Default") };            
            commands = await query.Handle();
            return commands;
        }

        public async Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType : BotCommand
        {
            var query = SqlQuery<TEntityType>.Instance with
                { sql = $"select * from ##BotCommands c INNER JOIN ##BotActionPayloads a ON c.Command = a.EntityAction where Command = '/{SqlQuery<TEntityType>.Sample.Command.ToLower()}' for json path "
                , connectionString = config.GetConnectionString("Default") };
            var result = (await query.Handle()).Where(w => w is TEntityType).Cast<TEntityType>().ToList();
            lastResult = result.FirstOrDefault();
            return result;
        }

        
    }
}

