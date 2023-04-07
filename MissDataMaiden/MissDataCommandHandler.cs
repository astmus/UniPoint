using MediatR;
using MissBot.Abstractions;
using MissBot.Extensions.Entities;
using MissBot.Handlers;
using MissCore.Entities;
using MissCore.Handlers;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    internal class MissDataCommandHandler : BaseHandleComponent, IAsyncBotCommandHandler
    {
        IMediator mm;
        public MissDataCommandHandler(IMediator mediator)
            => mm = mediator;

        bool isCommand;
        IBotServicesProvider sp;
        public override Task ExecuteAsync(IHandleContext context)
        {
            var update = context.GetAny<Update<MissDataMaid>>();
            string cmd = "cmd";
            if (isCommand = update.IsCommand)
            {
                sp = context.BotServices;               
                cmd = update.Message.GetCommandAndArgs().command;
            }
            return HandleAsync(context, string.Concat(cmd.AsSpan(0,1).ToString().ToUpper(), cmd.AsSpan(1)));;
        }

        Task HandleAsync(IHandleContext context, string command) => command switch
        {
            nameof(Disk) => HandleAsync<Disk>(context),
            nameof(List) => HandleAsync<List>(context),
            nameof(Info) => HandleAsync<Info>(context),
            _ => context.Get<AsyncHandler>()(context)
        };

        public Task HandleCommandAsync<TCommand>(IContext<TCommand> context) where TCommand : class, IBotCommandData
        {
            context.BotServices = sp;
            
            var handler = sp.GetRequiredService<IAsyncHandler<TCommand>>() as BotCommandHandler<TCommand>;
            return handler.HandleAsync(context);
        }

        public Task HandleAsync<TCommand>(IHandleContext context) where TCommand : class, IBotCommandData
        {
            var upd = context.GetAny<Update<MissDataMaid>>();

            var cmdCtx = context.BotServices.GetRequiredService<IContext<TCommand>>();
            cmdCtx.Set(upd);
            cmdCtx.Set(upd.Message);
            cmdCtx.Set(upd.Chat);
            return HandleCommandAsync<TCommand>(cmdCtx);
        }
    }
}
