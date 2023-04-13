
namespace MissBot.Abstractions
{
    public interface IContext<TScope> : IContext
    {
        void SetServices(IBotServicesProvider botServices);
        TScope Data { get; set; }
        IHandleContext Root { get; }
        IResponse<TScope> CreateResponse(TScope scopeData = default);
        ICommonUpdate CommonUpdate { get; }
        BotClientDelegate ClientDelegate { get; }
    }
}
