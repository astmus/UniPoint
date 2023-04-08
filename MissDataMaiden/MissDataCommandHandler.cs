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
            
            var cmdCtx = context.BotServices.GetRequiredService<IContext<TCommand>>();
            cmdCtx.BotServices = context.BotServices;
            cmdCtx.Data ??= context.Get<TCommand>() ?? ActivatorUtilities.GetServiceOrCreateInstance<TCommand>(context.BotServices);
            cmdCtx.Data.Params = data.args;
            cmdCtx.Set(upd);
            cmdCtx.Set(upd.Message);
            cmdCtx.Set(upd.Chat);

            var handler = context.BotServices.GetRequiredService<IAsyncHandler<TCommand>>();
            await handler.HandleAsync(cmdCtx);

            AsyncHandler next = context.Get<AsyncHandler>();            
            await next(context).ConfigureAwait(false);

            int i = 0;
        }
    }
}
