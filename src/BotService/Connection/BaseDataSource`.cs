using System.Runtime.CompilerServices;
using BotService.Connection.Async;
using MissBot.Abstractions.DataAccess.Async;
using MissBot.Entities;

namespace BotService.Connection
{
    public abstract class BaseDataSource<TData> where TData : class
    {
        public class AsyncItemsQueue<TUnit> : AsyncQueue<TUnit>, IAsyncItemsQueue<TUnit> where TUnit : class, TData
        {
            internal protected async Task<TUnit> PopItemAsync(CancellationToken cancellationToken)
            {
                await signal.WaitAsync(cancellationToken);
                items.TryDequeue(out var workItem);
                return workItem;
            }
            
            public void PushItem(TUnit unit)
                => QueueItem(unit);
        }

        protected abstract AsyncItemsQueue<TData> Items { get; init; }
        public async IAsyncEnumerable<TData> PendingItems([EnumeratorCancellation] CancellationToken cancelToken)
        {
            do
            {
                if (await Items.PopItemAsync(cancelToken) is TData update)
                    yield return update;
            } while (!cancelToken.IsCancellationRequested);
            yield break;
        }
    }
}
