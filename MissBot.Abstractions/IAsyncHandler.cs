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

    public interface ISetupHandler<T>
    {
        SetupHandler<T> SetupHandler { get; }
    }


    public interface IAsyncHandler<T> : IAsyncHandler
    {
        AsyncGenericHandler<T> GenericHandler { get; }
        Task HandleAsync(IContext<T> context);
    }

    public interface IAsyncUpdateHandler<T>
    {
        T Update { get; set; }
        Task HandleUpdateAsync<U>(U update, IContext<T> context) where U:T,IUpdateInfo;
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
