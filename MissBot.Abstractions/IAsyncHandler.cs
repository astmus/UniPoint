using MissBot.Abstractions.DataAccess;

namespace MissBot.Abstractions
{
    public interface IAsyncHandler
    {
        AsyncHandler AsyncHandler { get; }
    }

    public interface IAsyncBotCommandDispatcher
    {
        Task ExecuteAsync(IHandleContext context);
        Task HandleAsync<TCommand>(IHandleContext context) where TCommand :BotCommand, IBotCommandData;
    
    }
    
    public interface IAsyncHandler<T> : IAsyncHandler
    {
        Task HandleAsync(IContext<T> context);
    }
    public interface IAsyncEntityActionHandler<TAction> : IAsyncHandler<TAction> where TAction:class, IEntityAction
    {
        Task HandleActionAsync(TAction action, IHandleContext context, CancellationToken cancel = default);
    }


    public interface IAsyncUpdateHandler<T>
    {
        Task HandleUpdateAsync<U>(U update, IContext<T> context) where U:IUpdate<T>,IUpdateInfo;
    }


    public interface IBotUpdatesDispatcher
    {
        void Initialize(CancellationToken cancel = default);
    }

    public interface IBotUpdatesDispatcher<TUpdate> : IBotUpdatesDispatcher where TUpdate : IUpdateInfo
    {
        Func<TUpdate, string> ScopePredicate { get; set; }
        void PushUpdate(TUpdate update);
    }
}
