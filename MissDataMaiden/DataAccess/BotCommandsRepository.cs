using System;
using System.Linq.Expressions;
using BotService.Common;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissCore;
using MissCore.Collections;


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
            return commands;
        }

        public IEnumerable<TEntityType> GetAll<TEntityType>() where TEntityType : BotCommand
        {            
            return commands.Where(c => c is TEntityType).Cast<TEntityType>().ToList();
        }

        public Task<IEnumerable<BotCommand>> GetAllAsync()
        {
            commands = Context.BotCommands.ToList();
            return Task.FromResult(commands);
        }

        public  Task<TCommand> GetAsync<TCommand>() where TCommand : BotCommand
        {
            return Task.FromResult(Context.GetCommand<TCommand>());
        }

        public TCommand GetByName<TCommand>(string name) where TCommand : BotCommand
        {
            throw new NotImplementedException();
        }
    }
}

