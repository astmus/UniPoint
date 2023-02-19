namespace MissBot.Abstractions
{
    public interface IContextHandler<T> : IAsyncHandler where T: IUpdateInfo
    {
        void SetupContext(IContext context, T update);
    }
    public interface IHandleContext
    {
        IBotServicesProvider BotServices { get; }
        IContext ContextData { get; }
        T NextHandler<T>() where T : IAsyncHandler;
        IUpdateInfo Update { get; }
    }

    public interface IUpdateInfo
    {
        uint UpdateId { get; }
        bool IsHandled { get; set; }
    }
}
