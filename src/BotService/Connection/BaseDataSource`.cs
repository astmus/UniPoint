using System.Runtime.CompilerServices;
using BotService.Connection.Async;
using MissBot.Abstractions;
using MissCore.Data.Identity;

namespace BotService.Connection
{
    public abstract class BaseDataSource<TUpdate> where TUpdate : class, IUpdateInfo
    {
        public class AsyncUpdatesQueue<T> : AsyncQueue<T>, IAsyncUpdatesQueue<T> where T : class, TUpdate
        {
            public AsyncUpdatesQueue()
            {
                QueueId = Identity.Of(this);
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
                if (await Updates.PopUpdateAsync(cancelToken) is TUpdate update && !update.IsHandled)
                    yield return update;
            } while (!cancelToken.IsCancellationRequested);
            yield break;
        }
    }
}
