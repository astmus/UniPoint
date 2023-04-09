using Telegram.Bot.Types;

namespace MissBot.Abstractions
{
    public interface IContextHandler<T>
    {
        void SetupContext(IContext context, T update);
    }

    public interface IHandleContext : IContext
    {
        IAsyncHandler<T> GetAsyncHandler<T>();
        IBotServicesProvider BotServices { get; }        
        T NextHandler<T>() where T : class;
        IContext<T> GetContext<T>();
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
    }

    public interface IUpdateInfo
    {
        uint UpdateId { get; }
        bool IsHandled { get; set; }
    }
}
