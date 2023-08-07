using System.Collections.Concurrent;
using MissBot.Abstractions.DataAccess.Async;
using MissBot.Extensions;
using MissCore.Extensions;

namespace BotService.Connection.Async
{
	public class AsyncQueue<T> : ConcurrentQueue<T>, IAsyncQueue<T>
	{
		protected SemaphoreSlim signal = new SemaphoreSlim(0);
		/// <summary>
		/// protected AutoResetEvent reset = new AutoResetEvent(;
		/// </summary>
		/// <param name="update"></param>

		public void QueueItem(T update)
		{
			update.ThrowIfNull(nameof(update));
			Enqueue(update);
			signal.Release();
		}

		internal protected async Task<T> DequeueAsync(CancellationToken cancellationToken)
		{
			await signal.WaitAsync(cancellationToken);
			TryDequeue(out var workItem);
			return workItem;
		}
	}
}
