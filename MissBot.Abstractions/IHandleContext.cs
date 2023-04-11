using Telegram.Bot.Types;

namespace MissBot.Abstractions
{
    public interface IHandleContext : IContext
    {
        IBotServicesProvider BotServices { get; }        
        IAsyncHandler<T> GetAsyncHandler<T>();
        T GetNextHandler<T>() where T : class;
        IHandleContext SetNextHandler<T>(IContext context, T data) where T:class;
        IContext<T> CreateDataContext<T>(T data = default);
    }

    public interface IUpdate<TUpdate> : IUpdateInfo
    {
        TUpdate Data { get; }
    }

    public interface ICommonUpdate
    {
        Message Message { get; }
        Chat Chat { get; }
        bool IsHandled { get; set; }
    }

    public interface IUpdateInfo
    {
        uint UpdateId { get; }
        bool IsHandled { get; set; }
    }
}
