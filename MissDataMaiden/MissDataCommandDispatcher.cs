using MissBot.Abstractions;
using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataContext;
using MissBot.Abstractions.Entities;
using MissBot.Entities;
using MissBot.Extensions.Entities;
using MissCore.Data;
using MissDataMaiden.Commands;

namespace MissDataMaiden
{
    internal class MissDataCommandDispatcher : BaseHandler<BotCommand>, IAsyncBotCommandDispatcher
    {
        IRepository<BotCommand> commandsRepository;
        public MissDataCommandDispatcher(IRepository<BotCommand> mediator)
            => commandsRepository = mediator;

        bool isCommand;
        (string command, string[] args) data;

        public async Task ExecuteAsync(IHandleContext context)
        {
            var update = context.Any<Update<MissDataMaid>>();

            if (isCommand = update.IsCommand)
            {
                Context = context;
                data = context.TakeByKey<Message>().GetCommandAndArgs();
            }
            await HandleAsync(context, data.command);
        }

        Task HandleAsync(IHandleContext context, string command) => command switch
        {
            nameof(Disk) => HandleAsync<Disk>(context),
            nameof(List) => HandleAsync<List>(context),
            nameof(Info) => HandleAsync<Info>(context),
            _ => context.Get<AsyncHandler>()(context)
        };

        public async Task HandleAsync<TCommand>(IHandleContext context, CancellationToken cancel = default) where TCommand : BotCommand, IBotUnitCommand
        {
            var handler = context.GetAsyncHandler<TCommand>() as BotCommandHandler<TCommand>;
            handler.Context = context;
            var cmd = await commandsRepository.GetAsync<TCommand>();

            //var ctx = context.CreateDataContext<TCommand>();
            //ctx.Data ??= cmd;
            //var Data = ctx.Data as BotCommandUnit;
            //Data.Command = data.command;
            //Data.Params = data.args;

            await handler.HandleAsync(cmd, cancel);
            if (!context.IsHandled.HasValue)
                context.IsHandled = true;
        }

        public override Task HandleAsync(BotCommand data, CancellationToken cancel = default)
        {
            throw new NotImplementedException();
        }
    }
}
