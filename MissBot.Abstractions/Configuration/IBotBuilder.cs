using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess;
using MissBot.Abstractions.Bot;
using MissBot.Abstractions.Handlers;
using MissBot.Entities;
using MissBot.Entities.Query;
using MissBot.Entities.Results;
using MissBot.Entities.Abstractions;

namespace MissBot.Abstractions.Configuration
{
	public interface IBotBuilder : IBotUnitBuilder
	{
		IBotBuilder Use(Func<IHandleContext, AsyncHandler> component);
		IBotBuilder AddAction<TAction, THandler>() where THandler : class, IAsyncHandler<TAction> where TAction : class, IBotAction;
		IBotBuilder AddCommand<TCommand, THandler>() where THandler : BotCommandHandler<TCommand> where TCommand : BaseBotAction;
		IBotBuilder AddCustomCommandCreator<TCreator>() where TCreator : class, ICreateBotCommandHandler;
		IBotBuilder AddInputParametersHandler();
		IBotUnitBuilder AddResponseUnit<TUnit>() where TUnit : BaseBotUnit;
		AsyncHandler BuildHandler();
		void Build();
	}

	public interface IBotBuilder<TBot> : IBotBuilder where TBot : class, IBot
	{
		IBotBuilder<TBot> AddRepository<TRepository, TImplementatipon>() where TRepository : class, IRepository where TImplementatipon : class, TRepository;
		IBotBuilder<TBot> Use<THandler>() where THandler : class, IAsyncHandleComponent;
		IBotBuilder<TBot> UseInlineHandler<TUnit, THandler>() where THandler : class, IAsyncHandler<InlineQuery<TUnit>> where TUnit : BaseUnit, IUnit;
		IBotBuilder<TBot> UseCallbackDispatcher<THandler>() where THandler : class, IAsyncHandler<CallbackQuery>;
		IBotBuilder<TBot> UseInlineAnswerHandler<THandler>() where THandler : class, IAsyncHandler<ChosenInlineResult>;
		IBotBuilder<TBot> UseCommandDispatcher<THandler>() where THandler : class, IAsyncBotCommandDispatcher;
		IBotBuilder<TBot> UseUpdateHandler<THandler>() where THandler : class, IAsyncUpdateHandler<TBot>;
		IBotBuilder<TBot> UseCommandsRepository<THandler>() where THandler : class, IBotCommandsRepository;
		IBotBuilder<TBot> UseMessageHandler<THandler>() where THandler : class, IAsyncHandler<Message>;
	}
}
