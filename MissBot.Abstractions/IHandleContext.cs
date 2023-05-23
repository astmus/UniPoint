using MissBot.Abstractions.DataContext;
using MissBot.Entities;

namespace MissBot.Abstractions
{
    public interface IHandleContext : IContext
    {
        bool? IsHandled { get; set; }
        IBotServicesProvider BotServices { get; }
        IBotContext Bot { get; }
        IAsyncHandler<T> GetAsyncHandler<T>();
        T GetNextHandler<T>() where T : class;
        bool Contains<T>();
        AsyncHandler CurrentHandler { get; }
        IRequestProvider Provider { get; }
        IHandleContext SetNextHandler<T>(IContext context, T data) where T : class;
    }

    public interface IUpdate<TUpdate> : IUpdateInfo
    {
        TUpdate Data { get; }
    }

    public interface IUnitUpdate
    {
        Message CurrentMessage { get; }
        Chat Chat { get; }
        bool IsCommand { get; }
    }

    public interface IUpdateInfo
    {
        uint UpdateId { get; }        
    }
}
