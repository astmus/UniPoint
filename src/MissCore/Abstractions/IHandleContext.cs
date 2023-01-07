namespace MissCore.Abstractions
{

    public interface IHandleContext
    {
        IBotServicesProvider BotServices { get; }
        IContext Data { get; }
        IBotClient Client { get; }
        T NextHandler<T>() where T : IAsyncHandler;
        IUpdateInfo Info { get; }
    }

    public interface IUpdateInfo
    {
        long ChatId { get; }
        long UserId { get; }
        uint UpdateId { get; }
        bool IsHandled { get; set; }
    }
}
