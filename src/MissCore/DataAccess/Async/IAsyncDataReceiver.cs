namespace MissBot.Abstractions
{
    public interface IAsyncDataReceiver<TUpdate> : IAsyncEnumerable<TUpdate> where TUpdate : class, IUpdateInfo
    {

    }
}
