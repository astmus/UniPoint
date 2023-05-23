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

        public AsyncBotUpdatesDispatcher(IHandleContextFactory factory, ILogger<AsyncBotUpdatesDispatcher<TUpdate>> logger, IBotConnectionOptionsBuilder builder)
        {
            Factory = factory;
            log = logger;
            this.builder = builder;
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
                    var id = ScopePredicate(update);
                    if (id == null)
                        continue;
                    var scope = Factory.Init(id);
                    using (var cts = new CancellationTokenSource())
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<IAsyncUpdateHandler<TUpdate>>();
                        var ctx = scope.ServiceProvider.GetRequiredService<IContext<TUpdate>>();
                        ctx.SetData(update);
                        ctx.Set(scope.ServiceProvider);
                        var hctx = ctx as IHandleContext;
                        
                        await handler.HandleUpdateAsync(update, hctx, cts.Token).ConfigFalse();

                        if (hctx.IsHandled == true)
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

        CancellationTokenSource src;
        private readonly IBotConnectionOptionsBuilder builder;

        public void PushUpdate(TUpdate update)
            => Updates.QueueItem(update);

        public void Initialize(CancellationToken cancel = default)
        {
            src = CancellationTokenSource.CreateLinkedTokenSource(cancel, default);
            StartInThread();
        }
    }
}

