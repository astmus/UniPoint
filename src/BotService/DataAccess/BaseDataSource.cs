using System.Runtime.CompilerServices;
using BotService.DataAccess.Async;
using MissCore.Abstractions;
using MissCore.DataAccess.Async;

namespace BotService.DataAccess
{

    public abstract class BaseDataSource<TUpdate> where TUpdate : class
    {
        public class AsyncSourceUpdatesQueue : AsyncQueue<TUpdate>, IAsyncSourceUpdatesQueue<TUpdate>
        {
            internal protected async Task<TUpdate> PopUpdateAsync(CancellationToken cancellationToken)
            {
                await signal.WaitAsync(cancellationToken);
                items.TryDequeue(out var workItem);
                return workItem;
            }

            public void PushUpdate(TUpdate update)
                => QueueItem(update);

            public void PushUpdate<TGUpdate>(TGUpdate update) where TGUpdate : TUpdate
                => QueueItem(update);
            
        }
        protected abstract AsyncSourceUpdatesQueue Updates { get; }
        public async IAsyncEnumerable<TUpdate> PendingUpdates([EnumeratorCancellation] CancellationToken cancelToken)
        {
            do
            {
                TUpdate update = default(TUpdate);
                if ((update = await Updates.PopUpdateAsync(cancelToken)) != default(TUpdate))
                    yield return update;
            } while (!cancelToken.IsCancellationRequested);
            yield break;
        }

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            //LogInfo($"Bot finished works : {this}");
            return Task.CompletedTask;
        }


    }
}
