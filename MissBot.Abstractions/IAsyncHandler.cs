using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess.Async;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Entities.Abstractions;

namespace MissBot.Abstractions
{
	public interface IAsyncHandler
	{
		AsyncHandler AsyncDelegate { get; }
	}

	public interface IAsyncBotCommandDispatcher : IAsyncHandler
	{
		Task HandleBotCommandAsync<TCommand>(IHandleContext context, CancellationToken cancel = default) where TCommand : BotCommand, IBotAction;
	}

	public interface IAsyncUnitActionHanlder<TUnit> where TUnit : BaseUnit
	{
		void HandleUnitAction(IUnitAction<TUnit> action);
		Task HandleUnitActionAsync(IUnitAction<TUnit> action, CancellationToken cancel = default);
	}

	public interface IAsyncHandler<in T> : IAsyncHandler
	{
		Task HandleAsync(T data, CancellationToken cancel = default);
	}
	public interface IAsyncBotUnitActionHandler : IAsyncHandler
	{
		Task HandleAsync<TUnitAction>(Func<FormattableUnitBase, IHandleContext, Task> callBack, IBotAction<TUnitAction> action, IHandleContext context, CancellationToken cancel) where TUnitAction : class, IUnitEntity;
	}

	public interface ICreateBotCommandHandler
	{
		Task CreateAsync(IHandleContext context, CancellationToken cancel = default);
	}

	public interface ICreateBotCommandHandler<TCommand> : ICreateBotCommandHandler where TCommand : BotCommand
	{
	}

	public interface IAsyncHandleComponent
	{
		Task HandleAsync(IHandleContext context, AsyncHandler next, CancellationToken cancel = default);
	}

	public interface IAsyncUnitActionSource<TUnit> where TUnit : BaseBotUnit
	{
		void PushUnit(TUnit unit, IHandleContext context, CancellationToken cancel = default);
	}

	public interface IAsyncUpdateHandler<T> where T : class
	{
		Task HandleUpdateAsync(T update, IHandleContext context, CancellationToken cancel);
	}

	public interface IBotUpdatesDispatcher
	{
		void Initialize(CancellationToken cancel = default);
	}

	public interface IBotUpdatesDispatcher<TUpdate> : IBotUpdatesDispatcher, IAsyncUpdatesQueue<TUpdate> where TUpdate : Update
	{
		Func<TUpdate, string> ScopePredicate { get; set; }
	}
}
