using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Entities;
using MissBot.Extensions.Entities;
using MissCore.Data;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    internal class MissDataCommandDispatcher : IAsyncBotCommandDispatcher
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
                data = context.Take<Message>().GetCommandAndArgs();

            return HandleAsync(context, data.command);
        }

        Task HandleAsync(IHandleContext context, string command) => command switch
        {
            nameof(Disk) => HandleAsync<Disk>(context),
            nameof(List) => HandleAsync<List>(context),
            nameof(Info) => HandleAsync<Info>(context),
            _ => context.Get<AsyncHandler>()(context)
        };

        public async Task HandleAsync<TCommand>(IHandleContext context) where TCommand : BotCommand, IBotUnit
        {
            var handler = context.GetAsyncHandler<TCommand>() as BotCommandHandler<TCommand>;

            var cmd = await commandsRepository.GetAsync<TCommand>();

            //var ctx = context.CreateDataContext<TCommand>();
            //ctx.Data ??= cmd;
            //var Data = ctx.Data as BotCommandUnit;
            //Data.Command = data.command;
            //Data.Params = data.args;

            await handler.HandleAsync(cmd, context);

            AsyncHandler next = context.Handler;
            await next(context).ConfigureAwait(false);
        }
    }
}
