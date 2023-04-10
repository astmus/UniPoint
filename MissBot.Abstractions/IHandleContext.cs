using Telegram.Bot.Types;

namespace MissBot.Abstractions
{
    public interface IHandleContext : IContext
    {
        IAsyncHandler<T> GetAsyncHandler<T>();
        IBotServicesProvider BotServices { get; }        
        T NextHandler<T>() where T : class;
        IContext<T> GetContext<T>(T data = default);
        IHandleContext SetupData<T>(IContext context, T data);
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
