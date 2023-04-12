using MissBot.Abstractions.Persistance;

namespace MissBot.Abstractions
{
    public interface IAsyncHandler
    {
        ExecuteHandler ExecuteHandler { get; }
        AsyncHandler AsyncHandler { get; }
    }

    public interface IAsyncBotCommandHandler
    {
        Task ExecuteAsync(IHandleContext context);
        Task HandleAsync<TCommand>(IHandleContext context) where TCommand :class, IBotCommand;
    
    }

    public interface IAsyncHandler<T> : IAsyncHandler
    {
        Task HandleAsync(IContext<T> context);
    }
    public interface IAsyncEntityActionHandler<TEntity, TAction> : IAsyncHandler<TAction> where TAction:IEntityAction<TEntity>
    {
        Task HandleActionAsync(IRepository<TEntity> repository, IContext<TAction> context);
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
