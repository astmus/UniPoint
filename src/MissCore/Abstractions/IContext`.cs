namespace MissCore.Abstractions
{
    public interface IContext<TScope> : IContext
    {
        TScope Data { get; set; }
        IResponseChannel Channel {get;}
    }
}
