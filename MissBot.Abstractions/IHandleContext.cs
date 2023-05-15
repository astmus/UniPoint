using MissBot.Entities;

namespace MissBot.Abstractions
{
    public interface IHandleContext : IContext
    {
        bool? IsHandled { get; set; }
        IDataMap Map { get; }
        IBotServicesProvider BotServices { get; }        
        IAsyncHandler<T> GetAsyncHandler<T>();
        T GetNextHandler<T>() where T : class;
        AsyncHandler Handler { get; }
        IRequestProvider Provider { get; }
        IHandleContext SetNextHandler<T>(IContext context, T data) where T : class;
        IResponse<TScope> CreateResponse<TScope>();
    }

    public interface IUpdate<TUpdate> : IUpdateInfo
    {
        TUpdate Data { get; }
    }

    public interface ICommonUpdate
    {
        Message Message { get; }
        Chat Chat { get; }
        bool? IsHandled { get; set; }
    }

    public interface IUpdateInfo
    {
        uint UpdateId { get; }
        bool? IsHandled { get; set; }
    }
}
