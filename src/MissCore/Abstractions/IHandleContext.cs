using System;

namespace MissCore.Abstractions
{

    public interface IHandleContext
    {
        IServiceProvider BotServices { get; }
        IContext ContextData { get; }
        T NextHandler<T>() where T : IAsyncHandler;
        IUpdateInfo Update { get; }
    }

    public interface IUpdateInfo
    {
        string GetId();
        long ChatId { get; }
        long UserId { get; }
        uint UpdateId { get; }
        bool IsHandled { get; set; }
    }
}
