
namespace MissBot.Abstractions
{
    public interface IContext<TScope> : IContext
    {
        void SetServices(IBotServicesProvider botServices);        
        TScope Data { get; set; }
        IHandleContext Root { get; set; }
        IResponse Response { get; }
        ICommonUpdate CommonUpdate { get; }
        BotClientDelegate ClientDelegate { get; }
    }
}
