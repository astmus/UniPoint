using MissBot.Abstractions;
using MissBot.Entities;

namespace MissBot.Abstractions.DataAccess.Async
{
    public interface IBotUpdatesReceiver<out TUpdate> : IAsyncEnumerable<TUpdate> where TUpdate : Update
    {

    }

    public interface IAsyncUpdatesQueue<in TUpdate> where TUpdate : Update
    {
        void PushUpdate(TUpdate updade);
    }
    public interface IAsyncItemsQueue<in TUnit> where TUnit : class
    {
        void PushItem(TUnit unit);
    }
}
