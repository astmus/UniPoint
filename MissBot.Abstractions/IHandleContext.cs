using Telegram.Bot.Types;

namespace MissBot.Abstractions
{
    public interface IContextHandler<T>  where T: IUpdateInfo
    {
        void SetupContext(IContext context, T update);
    }

    public interface IHandleContext : IContext
    {        
        IBotServicesProvider BotServices { get; set; }        
        T NextHandler<T>() where T : class;
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
