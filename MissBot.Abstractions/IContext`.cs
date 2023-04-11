
namespace MissBot.Abstractions
{
    public interface IContext<TScope> : IContext
    {
        void SetServices(IBotServicesProvider botServices);
        TScope InitContextData(params object[] args);
        TScope Data { get; set; }
        IHandleContext Root { get; }
        IResponse<TScope> CreateResponse(TScope scopeData = default);
        ICommonUpdate CommonUpdate { get; }
        BotClientDelegate ClientDelegate { get; }
    }
}
