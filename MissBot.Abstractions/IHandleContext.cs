using MissBot.Abstractions.DataAccess;
using MissBot.Entities;

namespace MissBot.Abstractions
{
    public interface IHandleContext : IContext
    {
        AsyncHandler CurrentHandler { get; }
        bool Contains<T>(Id<T> identifier);
        bool? IsHandled { get; set; }
        IAsyncHandler<T> GetAsyncHandler<T>();
        IBotContext Bot { get; }
        IBotServicesProvider BotServices { get; }
        IHandleContext SetNextHandler<T>(IContext context, T data) where T : class;
        IRequestProvider Provider { get; }
        T GetBotService<T>() where T : class;
    }

    public interface IUpdate<TUpdate> : IUpdateInfo
    {
        TUpdate Data { get; }
    }

    public interface IUnitUpdate
    {
        string StringContent { get; }
        Message CurrentMessage { get; }
        Chat Chat { get; }
        bool IsCommand { get; }
    }

    public interface IUpdateInfo
    {
        uint UpdateId { get; }        
    }
}
