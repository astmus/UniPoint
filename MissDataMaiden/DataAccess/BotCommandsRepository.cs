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
using MissBot.Abstractions.Entities;
using MissBot.DataAccess;
using MissCore.Bot;
using MissDataMaiden.Queries;
using Newtonsoft.Json;

namespace MissDataMaiden.DataAccess
{
    public class BotCommandsRepository : SqlRepository, IBotCommandsRepository
    {
        
        IEnumerable<BotAction> commands;

        public BotCommandsRepository(IConfiguration configuration) : base(configuration)
        {
        }

        public IEnumerable<BotAction> GetAll()
        {
            GetAllAsync().RunSynchronously();
            return commands;
        }

        public IEnumerable<TEntityType> GetAll<TEntityType>() where TEntityType : BotAction
        {
            if (commands == null)
                GetAllAsync<TEntityType>().Wait();
            return commands.Where(c => c is TEntityType).Cast<TEntityType>().ToList();;
        }

        public async Task<IEnumerable<BotAction>> GetAllAsync()
        {
            commands = await HandleScalarQueryAsync<Unit<BotAction>>(BotContext.AllCommands.Request);
            return commands;
        }

        public async Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType : BotAction
        {
            //var query = SqlQuery<TEntityType>.Instance with
            //{ sql = SQL<BotCommand>.Sample.Command };//  .Query.cmd};

            var result = await HandleScalarQueryAsync<Unit<TEntityType>>(BotContext.AllCommands.Request);
            //var result = (await query.Handle()).Where(w => w is TEntityType).Cast<TEntityType>().ToList();
            //lastResult = result.FirstOrDefault();
            return result;
        }

        public async Task<TEntityType> GetAsync<TEntityType>() where TEntityType : BotAction
        {            
            var sql = BotContext.Command<TEntityType>(Unit<TEntityType>.Sample).Request;
            sql.Type = SQLJson.Path;
            var cmd = await HandleScalarQueryAsync<Unit<TEntityType>>(sql);
            return cmd.FirstOrDefault();
        }

        public TCommand GetByName<TCommand>(string name) where TCommand : BotAction
        {
            throw new NotImplementedException();
        }
    }
}

