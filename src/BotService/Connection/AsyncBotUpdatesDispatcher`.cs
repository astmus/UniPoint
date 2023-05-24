using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Entities;
using MissBot.Extensions;

namespace BotService.Connection
{
    public class AsyncBotUpdatesDispatcher<TUpdate> : BaseDataSource<TUpdate>, IBotUpdatesDispatcher<TUpdate> where TUpdate : Update
    {
        IHandleContextFactory Factory { get; }
        public ILogger<AsyncBotUpdatesDispatcher<TUpdate>> log { get; protected set; }
        Thread thread;
        protected override AsyncUpdatesQueue<TUpdate> Updates { get; init; }
        public Func<TUpdate, string> ScopePredicate { get; set; }
        CancellationTokenSource src;

        public AsyncBotUpdatesDispatcher(IHandleContextFactory factory, ILogger<AsyncBotUpdatesDispatcher<TUpdate>> logger)
        {
            Factory = factory;
            log = logger;
            Updates = new AsyncUpdatesQueue<TUpdate>();
            thread = new Thread(StartInThread);
            thread.IsBackground = true;
        }

        protected async void StartInThread()
        {
            try
            {
                await foreach (var update in PendingUpdates(src.Token))
                {
                    if (ScopePredicate(update) is not string id) continue;
                    var scope = Factory.Init(id);

                    using (var cts = new CancellationTokenSource())
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<IAsyncUpdateHandler<TUpdate>>();
                        var ctx = scope.ServiceProvider.GetRequiredService<IContext<TUpdate>>();

                        ctx.SetData(update);
                        
                        await handler.HandleUpdateAsync(update, (IHandleContext)ctx, cts.Token).ConfigFalse();

                        if (ctx.IsHandled == true)
                            Factory.Remove(id);
                    }
                };
            }
            catch (Exception e)
            {
                log.WriteCritical(e);
            }
            finally
            {
            if (!src.IsCancellationRequested)
                StartInThread();
            }
        }


        public void PushUpdate(TUpdate update)
            => Updates.QueueItem(update);

        public void Initialize(CancellationToken cancel = default)
        {
            src = CancellationTokenSource.CreateLinkedTokenSource(cancel, default);
            StartInThread();
        }
    }
}

