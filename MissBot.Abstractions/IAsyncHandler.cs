using MissBot.Abstractions.Actions;
using MissBot.Abstractions.DataAccess.Async;
using MissBot.Abstractions.Entities;
using MissBot.Entities;

namespace MissBot.Abstractions
{
    public interface IAsyncHandler
    {
        AsyncHandler AsyncDelegate { get; }
    }

    public interface IAsyncBotCommandDispatcher : IAsyncHandler
    {        
        Task HandleBotCommandAsync<TCommand>(IHandleContext context, CancellationToken cancel = default) where TCommand : BotCommand, IBotUnitAction; 
    }

    public interface IAsyncHandler<in T> : IAsyncHandler
    {
        Task HandleAsync(T data, CancellationToken cancel = default);
    }
    public interface IAsyncBotUnitActionHandler : IAsyncHandler
    {
        Task HandleAsync<TUnitAction>(Func<FormattableUnitBase, IHandleContext, Task> callBack, IBotUnitAction<TUnitAction> action, IHandleContext context, CancellationToken cancel) where TUnitAction : BaseUnit;
    }
    public interface ICreateBotCommandHandler
    {
        Task CreateAsync(IHandleContext context, CancellationToken cancel = default);
        //Task StartCreate<TCommand>(TCommand emptyCommand, IHandleContext context, CancellationToken cancel = default) where TCommand:BotCommand;
    }
    public interface ICreateBotCommandHandler<TCommand> : ICreateBotCommandHandler where TCommand:BotCommand
    {
    }

    public interface IAsyncHandleComponent
    {
        Task HandleAsync(IHandleContext context, AsyncHandler next, CancellationToken cancel = default);
    }
    public interface IAsyncUnitActionSource<TUnit>  where TUnit : BaseUnit
    {
        void PushUnit(TUnit unit, IHandleContext context, CancellationToken cancel = default);
        //Task HandleActionAsync(TUnit action, IHandleContext context, CancellationToken cancel = default);
    }            

    public interface IAsyncUpdateHandler<T> where T: class
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
