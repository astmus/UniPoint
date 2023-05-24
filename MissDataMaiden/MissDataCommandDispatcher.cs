using MissBot.Abstractions;
using MissBot.Abstractions.DataContext;
using MissBot.Abstractions.Entities;
using MissBot.Handlers;
using MissCore.Collections;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    internal class MissDataCommandDispatcher : BaseBotCommandDispatcher
    {
        IRepository<BotCommand> commandsRepository;
        public MissDataCommandDispatcher(IRepository<BotCommand> repository)
            => commandsRepository = repository;

        protected override Task HandleAsync(IHandleContext context, string command) => command switch
        {
            nameof(Disk) => HandleBotCommandAsync<Disk>(context),
            nameof(List) => HandleBotCommandAsync<List>(context),
            nameof(Info) => HandleBotCommandAsync<Info>(context),
            _ => HandleCommonAsync(context)
        };

        public override async Task HandleBotCommandAsync<TCommand>(IHandleContext context, CancellationToken cancel = default)
        {
            if (context.GetAsyncHandler<TCommand>() is BotCommandHandler<TCommand> handler)
            {
                

                var command = await commandsRepository.GetAsync<TCommand>();
                
                await handler.HandleAsync(command, cancel);               
            }
        }

        protected async override Task HandleCommonAsync(IHandleContext context)
        {
            if (Current.args.FirstOrDefault() is string raw)
            {
                var request = context.Provider.FromRaw<BotCommand>(raw);
                var repository = context.GetBotService<IJsonRepository>();
                var result = await repository.HandleReadAsync(request);
                var response = Context.BotServices.Response<BotCommand>();
                foreach (var item in result.SupplyTo<Unit<BotCommand>>())
                {
                    response.Write(item);
                    if (response.Length > 1500)
                        await response.Commit(default);
                }
                await response.Commit(default);
            }
           // response.Write(result); 
        }
    }
}
