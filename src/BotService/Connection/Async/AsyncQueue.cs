using System.Collections.Concurrent;
using MissBot.Extensions;
using MissCore.DataAccess.Async;

namespace BotService.Connection.Async
{
    public class AsyncQueue<T> : IAsyncQueue<T>
    {
        protected ConcurrentQueue<T> items = new ConcurrentQueue<T>();
        protected SemaphoreSlim signal = new SemaphoreSlim(0);
        /// <summary>
        /// protected AutoResetEvent reset = new AutoResetEvent(;
        /// </summary>
        /// <param name="update"></param>

        public void QueueItem(T update)
        {        
            update.ThrowIfNull(nameof(update));
            items.Enqueue(update);
            signal.Release();
        }

        internal protected async Task<T> DequeueAsync(CancellationToken cancellationToken)
        {
            await signal.WaitAsync(cancellationToken);
            items.TryDequeue(out var workItem);
            return workItem;
        }
    }
}
