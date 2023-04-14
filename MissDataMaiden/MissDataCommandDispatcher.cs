using MediatR;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Extensions.Entities;
using MissBot.Handlers;
using MissCore.Entities;
using MissCore.Handlers;
using MissDataMaiden.Commands;
using Telegram.Bot.Types;

namespace MissDataMaiden
{
    internal class MissDataCommandDispatcher :  IAsyncBotCommandDispatcher
    {
        IRepository<BotCommand> commandsRepository;
        public MissDataCommandDispatcher(IRepository<BotCommand> mediator)
            => commandsRepository = mediator;

        bool isCommand;        
        (string command, string[] args) data;

        public Task ExecuteAsync(IHandleContext context)
        {
            var update = context.Any<Update<MissDataMaid>>();
            
            if (isCommand = update.IsCommand)            
                data = update.Message.GetCommandAndArgs();            

            return HandleAsync(context, data.command);
        }

        Task HandleAsync(IHandleContext context, string command) => command switch
        {
            nameof(Disk) => HandleAsync<Disk>(context),
            nameof(List) => HandleAsync<List>(context),
            nameof(Info) => HandleAsync<Info>(context),
            _ => context.Get<AsyncHandler>()(context)
        };  

        public async Task HandleAsync<TCommand>(IHandleContext context) where TCommand :BotCommand, IBotCommandData
        {
            var handler = context.GetAsyncHandler<TCommand>() as BotCommandHandler<TCommand>;
            var cmd = handler.Command;
            var cmds =await commandsRepository.GetAllAsync<TCommand>();
            
            var ctx = context.CreateDataContext<TCommand>();
            
            ctx.Data ??= cmds.FirstOrDefault();
            ctx.Data.Command = data.command;
            ctx.Data.Params = data.args;

            await handler.HandleAsync(ctx);

            AsyncHandler next = context.Get<AsyncHandler>();            
            await next(context).ConfigureAwait(false);
        }
    }
}
