
namespace MissBot.Abstractions
{
    public interface IContext<TScope> : IContext
    {
        string Key { get; }
        void SetData(TScope data);
        TScope Data { get; set; }        
    }
}
