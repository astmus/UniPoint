
namespace MissBot.Abstractions
{
    public interface IContext<TScope> : IContext
    {
        void SetData(TScope data);
        bool? IsHandled { get; set; }
    }
}
