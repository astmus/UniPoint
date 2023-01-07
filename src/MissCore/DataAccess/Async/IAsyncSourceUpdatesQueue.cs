namespace MissCore.DataAccess.Async
{
    public interface IAsyncSourceUpdatesQueue<in T> where T : class
    {
        void PushUpdate<TUpdate>(TUpdate updade) where TUpdate : T;
    }
}
