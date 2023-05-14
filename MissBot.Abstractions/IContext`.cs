
namespace MissBot.Abstractions
{
    public interface IContext<TScope> : IContext
    {
        void SetData(TScope data);
        TScope Data { get; set; }
        DataMap Map { get; }
        IHandleContext Root { get; }
        IResponse<TSub> CreateResponse<TSub>(TScope scopeData = default) where TSub : TScope;
    }
}
