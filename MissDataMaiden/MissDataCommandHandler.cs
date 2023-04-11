using MediatR;
using MissBot.Abstractions;
using MissBot.Extensions.Entities;
using MissBot.Handlers;
using MissCore.Entities;
using MissCore.Handlers;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    internal class MissDataCommandHandler :  IAsyncBotCommandHandler
    {
        IMediator mm;
        public MissDataCommandHandler(IMediator mediator)
            => mm = mediator;

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

        public async Task HandleAsync<TCommand>(IHandleContext context) where TCommand :class, IBotCommand
        {            
            var upd = context.Any<ICommonUpdate>();
            
            var ctx = context.CreateDataContext<TCommand>();
            
            var handler = context.GetAsyncHandler<TCommand>() as BotCommandHandler<TCommand>;
            ctx.Data ??= handler.Command;
            ctx.Data.Params = data.args;

            await handler.HandleAsync(ctx);

            AsyncHandler next = context.Get<AsyncHandler>();            
            await next(context).ConfigureAwait(false);
        }
    }
}
