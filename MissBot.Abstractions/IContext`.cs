
namespace MissBot.Abstractions
{
    public interface IContext<TScope> : IContext
    {
        TScope Data { get; set; }
        IResponseChannel Response { get; }
    }
}
