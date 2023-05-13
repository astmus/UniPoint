
namespace MissBot.Abstractions
{
    public interface IContext<TScope> : IContext
    {
        void SetData(TScope data);
        TScope Data { get; set; }
        DataMap Map { get; }
        IHandleContext Root { get; }
        IResponse<TScope> CreateResponse(TScope scopeData = default);
        ICommonUpdate CommonUpdate { get; }
        BotClientDelegate ClientDelegate { get; }
    }
}
