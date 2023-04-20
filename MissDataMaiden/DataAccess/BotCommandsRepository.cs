using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BotService;
using MediatR;
using Microsoft.Data.SqlClient;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.DataAccess;
using MissBot.DataAccess.Sql;
using MissCore.Bot;
using MissDataMaiden.Queries;
using Newtonsoft.Json;
using Telegram.Bot.Types;

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
            commands = await HandleScalarQueryAsync<List<BotCommand>>(BotContext.AllCommands.Request);
            return commands;
        }

        public async Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType : BotCommand
        {
            //var query = SqlQuery<TEntityType>.Instance with
            //{ sql = SQL<BotCommand>.Sample.Command };//  .Query.cmd};

            var result = await HandleScalarQueryAsync<List<TEntityType>>(BotContext.AllCommands.Request);
            //var result = (await query.Handle()).Where(w => w is TEntityType).Cast<TEntityType>().ToList();
            //lastResult = result.FirstOrDefault();
            return result;
        }

        public async Task<TEntityType> GetAsync<TEntityType>() where TEntityType : BotCommand
        {            
            var sql = BotContext.Command<TEntityType>(Unit<TEntityType>.Sample);
            sql.Request.Type = SQLJson.Path;
            var cmd = await HandleScalarQueryAsync<Unit<TEntityType>>(sql.Request);
            return cmd.Content.FirstOrDefault();
        }

        public TCommand GetByName<TCommand>(string name) where TCommand : BotCommand
        {
            throw new NotImplementedException();
        }
    }
}

