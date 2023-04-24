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
using MissBot.DataAccess.Sql;
using MissCore.Bot;
using MissDataMaiden.Queries;
using Newtonsoft.Json;

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
            //var query = SqlQuery<TEntityType>.Instance with
            //{ sql = SQL<BotCommand>.Sample.Command };//  .Query.cmd};

            var result = await HandleSqlQueryAsync<Unit<TEntityType>.Collection>(BotContext.AllCommands.Command);
            //var result = (await query.Handle()).Where(w => w is TEntityType).Cast<TEntityType>().ToList();
            //lastResult = result.FirstOrDefault();
            return result;
        }

        public async Task<TEntityType> GetAsync<TEntityType>() where TEntityType : BotCommand
        {
            
            var sql = SQL.Command<TEntityType>(); // BotContext.Command<TEntityType>(Unit<TEntityType>.Sample).Command;
            var cmd = await HandleSqlQueryAsync<BotEntity<TEntityType>.Collection>(sql);
            return cmd.FirstOrDefault();
        }

        public TCommand GetByName<TCommand>(string name) where TCommand : BotCommand
        {
            throw new NotImplementedException();
        }
    }
}

