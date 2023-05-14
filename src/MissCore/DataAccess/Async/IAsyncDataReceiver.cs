using MissBot.Abstractions;

namespace MissCore.DataAccess.Async
{
    public interface IAsyncDataReceiver<TUpdate> : IAsyncEnumerable<TUpdate> where TUpdate : class, IUpdateInfo
    {

    }
}
