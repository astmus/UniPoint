namespace MissCore.DataAccess.Async
{
    public interface IAsyncQueue<in T>
    {
        void QueueItem(T item);
    }

    public interface IAsyncGeneicQueue<in TBase> where TBase : class
    {
        void QueueBasedItem<TItem>(TItem item) where TItem : class, TBase;
    }
}
