using MissBot.Abstractions;
using MissBot.Entities;

namespace MissCore.DataAccess.Async
{
    public interface IBotUpdatesReceiver<out TUpdate> : IAsyncEnumerable<TUpdate> where TUpdate : Update
    {

    }

    public interface IAsyncUpdatesQueue<in TUpdate> where TUpdate : Update
    {
        void PushUpdate(TUpdate updade);
    }
}
