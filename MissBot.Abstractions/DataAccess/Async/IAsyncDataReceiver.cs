using MissBot.Abstractions;

namespace MissBot.Abstractions.DataAccess.Async
{
    public interface IAsyncDataReceiver<TUpdate> : IAsyncEnumerable<TUpdate> where TUpdate : class, IUpdateInfo
    {

    }
}
