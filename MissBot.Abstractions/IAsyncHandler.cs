using MissBot.Abstractions.Actions;
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
        Task HandleBotCommandAsync<TCommand>(IHandleContext context, CancellationToken cancel = default) where TCommand : BotCommand, IBotUnitCommand; 
    }

    public interface IAsyncHandler<T> : IAsyncHandler
    {
        Task HandleAsync(T data, CancellationToken cancel = default);
    }
    public interface IAsyncEntityActionHandler<TAction> : IAsyncHandler<TAction> where TAction : class, IBotUnitCommand
    {
        Task HandleActionAsync(TAction action, IHandleContext context, CancellationToken cancel = default);
    }            

    public interface IAsyncUpdateHandler<T> where T: class
    {
        Task HandleUpdateAsync(T update, IHandleContext context, CancellationToken cancel);
    }

    public interface IBotUpdatesDispatcher
    {
        void Initialize(CancellationToken cancel = default);
    }

    public interface IBotUpdatesDispatcher<TUpdate> : IBotUpdatesDispatcher where TUpdate : Update
    {
        Func<TUpdate, string> ScopePredicate { get; set; }
        void PushUpdate(TUpdate update);
    }
}
