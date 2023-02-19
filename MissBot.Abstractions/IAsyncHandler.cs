namespace MissBot.Abstractions
{
    public interface IAsyncHandler
    {
        Task ExecuteAsync(IHandleContext context, HandleDelegate next);
    }

    public interface IAsyncBotCommansHandler
    {
        Task HandleCommandAsync(IHandleContext context, IBotCommandData command);
    }

    public interface IAsyncHandler<T>
    {
        Task HandleAsync(IContext<T> context, T data);
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
