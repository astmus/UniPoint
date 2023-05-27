using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Handlers;
using MissCore.Collections;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    internal class MissDataCommandDispatcher : BaseBotCommandDispatcher
    {
        IBotCommandsRepository commandsRepository;
        public MissDataCommandDispatcher(IBotCommandsRepository repository)
            => commandsRepository = repository;

        protected override Task HandleAsync(IHandleContext context, string command) => command switch
        {
            nameof(Disk)
                => HandleBotCommandAsync<Disk>(context),
            nameof(List)
                => HandleBotCommandAsync<List>(context),
            //nameof(Info)
            //    => HandleBotCommandAsync<Info>(context),
            _
                => base.HandleAsync(context, command)
        };

        public override async Task HandleBotCommandAsync<TCommand>(IHandleContext context, CancellationToken cancel = default)
        {
            if (context.GetAsyncHandlerOf<TCommand>() is BotCommandHandler<TCommand> handler)
            { 
                var command = await commandsRepository.GetAsync<TCommand>();
                handler.SetContext(Context);
                await handler.HandleAsync(command, cancel);               
            }
        }
        
        public override async Task HandleAsync(BotCommand command, CancellationToken cancel = default)
        {
            switch (command.Action)
            {
                case "add":
                    var handler = Context.GetBotService<IAsyncHandler<AddCommandHadler>>();
                    Context.Set(handler.AsyncDelegate);
                    await handler.AsyncDelegate(Context);
                    Context.IsHandled = false;
                    break;
                default:
                    var request = Context.Provider.FromRaw<BotCommand>(command.Payload);
                    var repository = Context.GetBotService<IJsonRepository>();
                    var result = await repository.ReadAsync(request);
                    var response = Context.BotServices.Response<BotCommand>();
                    foreach (var item in result.SupplyTo<Unit<BotCommand>>())
                    {
                        response.Write(item);
                        if (response.Length > 1500)
                            await response.Commit(default);
                    }
                    await response.Commit(default);
                    break;
            }           
        }
    }
}
