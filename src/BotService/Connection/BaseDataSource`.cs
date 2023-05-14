using System.Runtime.CompilerServices;
using BotService.Connection.Async;
using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissBot.Entities;
using MissCore.DataAccess.Async;

namespace BotService.Connection
{
    public abstract class BaseDataSource<TUpdate> where TUpdate : Update
    {
        public class AsyncUpdatesQueue<T> : AsyncQueue<T>, IAsyncUpdatesQueue<T> where T : class, TUpdate
        {
            public AsyncUpdatesQueue()
            {

            }

            public Identifier QueueId { get; protected set; }

            internal protected async Task<T> PopUpdateAsync(CancellationToken cancellationToken)
            {
                await signal.WaitAsync(cancellationToken);
                items.TryDequeue(out var workItem);
                return workItem;
            }

            public void PushUpdate(T update)
                => QueueItem(update);
        }
        protected abstract AsyncUpdatesQueue<TUpdate> Updates { get; init; }
        public async IAsyncEnumerable<TUpdate> PendingUpdates([EnumeratorCancellation] CancellationToken cancelToken)
        {
            do
            {
                if (await Updates.PopUpdateAsync(cancelToken) is TUpdate update)
                    yield return update;
            } while (!cancelToken.IsCancellationRequested);
            yield break;
        }
    }
}
