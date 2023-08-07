using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Handlers;
using MissCore.Data.Collections;
using MissCore.Handlers;
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
				case "Add":
					var handler = Context.GetBotService<ICreateBotCommandHandler>();
					await handler.CreateAsync(Context, cancel);
					Context.IsHandled = false;
					break;
				default:

					var repository = Context.GetBotService<IJsonRepository>();
					var result = await repository.RawAsync<GenericUnit>(command.Format);
					//var response = Context.BotServices.Response<BotCommand>();
					//foreach (IUnit<GenericUnit> item in result.Content)
					//{
					//response.Write(item.ToUnit<BotCommand>());
					if (Response.Length > 1500)
						await Response.Commit(default);
					//}
					await Response.Commit(default);
					break;
			}
		}
	}
}
