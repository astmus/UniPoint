namespace MissCore.Abstractions
{

    public interface IHandleContext
    {
        IBotServicesProvider BotServices { get; }
        IContext Data { get; }
        T NextHandler<T>() where T : IAsyncHandler;
        IUpdateInfo Info { get; }
    }

    public interface IUpdateInfo
    {
        string GetId();
        long ChatId { get; }
        long UserId { get; }
        uint UpdateId { get; }
        bool IsHandled { get; set; }
    }

    public interface IUpdateInfo<out T>
    {
        T GetId();
      
    }
}
