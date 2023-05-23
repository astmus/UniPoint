using MissBot.Abstractions;
using MissBot.Abstractions.DataContext;
using MissBot.Abstractions.Entities;
using MissBot.Handlers;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    internal class MissDataCommandDispatcher : BaseBotCommandHandler
    {
        IRepository<BotCommand> commandsRepository;
        public MissDataCommandDispatcher(IRepository<BotCommand> mediator)
            => commandsRepository = mediator;

        protected override Task HandleAsync(IHandleContext context, string command) => command switch
        {
            nameof(Disk) => HandleBotCommandAsync<Disk>(context),
            nameof(List) => HandleBotCommandAsync<List>(context),
            nameof(Info) => HandleBotCommandAsync<Info>(context),
            _ => HandleBotCommandAsync<BotCommand>(context)
        };

        public override async Task HandleBotCommandAsync<TCommand>(IHandleContext context, CancellationToken cancel = default)
        {
            if (context.GetAsyncHandler<TCommand>() is BotCommandHandler<TCommand> handler)
            {
                handler.Context = context;
                var command = await commandsRepository.GetAsync<TCommand>();
                
                await handler.HandleAsync(command, cancel);

                if (!context.IsHandled.HasValue)
                    context.IsHandled = true;
            }
        }     
    }
}
