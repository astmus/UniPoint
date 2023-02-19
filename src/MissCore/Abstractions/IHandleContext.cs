using System;
using MissCore.Entities;

namespace MissCore.Abstractions
{
    public interface IContextHandler<T> : IAsyncHandler
    {
        void SetupContext(IContext context, Update<T> update);
    }
    public interface IHandleContext
    {
        IBotServiceProvider BotServices { get; }
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
