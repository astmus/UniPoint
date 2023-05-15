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
            var info = BotUnit<BotCommand>.GetRequestInfo(d
                                => new[] { d.Command, d.Description });
            var cmd = Context.Provider.Request<BotCommand>(info);
            commands = await Context.HandleRequestAsync<Unit<BotCommand>.Collection>(cmd);
            return commands;
        }

        public async Task<IEnumerable<TEntityType>> GetAllAsync<TEntityType>() where TEntityType : BotCommand
        {
            var cmd = Context.Provider.Request<TEntityType>();
            var result = await Context.HandleRequestAsync<Unit<TEntityType>.Collection>(cmd);

            return result;
        }
        Expression<Func<BotCommand, string[]>> selector =>
            d =>new[] { d.Command, d.Description };

        public async Task<TEntityType> GetAsync<TEntityType>() where TEntityType : BotCommand
        {
            var cmd = Context.Provider.Request<TEntityType>();
            var result = await Context.HandleRequestAsync<TEntityType>(cmd);
            return result;
        }

        public TCommand GetByName<TCommand>(string name) where TCommand : BotCommand
        {
            throw new NotImplementedException();
        }
    }
}

