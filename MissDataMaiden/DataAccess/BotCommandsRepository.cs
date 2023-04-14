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

        public async Task<IEnumerable<BotCommand>> GetAllAsync()
        {
            var query = SqlQuery<BotCommand>.Instance with { sql = "SELECT * FROM ##BotCommands FOR JSON AUTO", connectionString = config.GetConnectionString("Default") };            
            commands = await query.Handle();
            return commands;
        }
        public record BotCommandsRequest(string sql, string connectionString) : SqlQuery<BotCommand>.Request(sql, connectionString);
        
    }
}

