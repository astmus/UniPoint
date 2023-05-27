using MissBot.Abstractions;
using MissBot.Entities;
using MissBot.Extensions;

namespace BotService.Connection
{
    public class AsyncBotUpdatesDispatcher<TUpdate> : BaseDataSource<TUpdate>, IBotUpdatesDispatcher<TUpdate> where TUpdate : Update
    {
        IHandleContextFactory Factory { get; }
        public ILogger<AsyncBotUpdatesDispatcher<TUpdate>> log { get; protected set; }
        Thread thread;
        protected override AsyncItemsQueue<TUpdate> Items { get; init; }
        public Func<TUpdate, string> ScopePredicate { get; set; }
        CancellationTokenSource src;

        public AsyncBotUpdatesDispatcher(IHandleContextFactory factory, ILogger<AsyncBotUpdatesDispatcher<TUpdate>> logger)
        {
            Factory = factory;
            log = logger;
            Items = new AsyncItemsQueue<TUpdate>();
            thread = new Thread(StartInThread);
            thread.IsBackground = true;
        }

        protected async void StartInThread()
        {
            try
            {
                await foreach (var update in PendingItems(src.Token))
                {
                    if (ScopePredicate(update) is not string id) continue;
                    var scope = Factory.Init(id);

                    using (var cts = new CancellationTokenSource())
                    {
                        var handler = scope.ServiceProvider.GetRequiredService<IAsyncUpdateHandler<TUpdate>>();
                        var ctx = scope.ServiceProvider.GetRequiredService<IContext<TUpdate>>().SetData(update);
                        if (ctx is IHandleContext context)
                        {
                            await handler.HandleUpdateAsync(update, context, cts.Token).ConfigFalse();

                            if (ctx.IsHandled == true) Factory.Remove(id);                          
                        }
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
            => Items.QueueItem(update);

        public void Initialize(CancellationToken cancel = default)
        {
            src = CancellationTokenSource.CreateLinkedTokenSource(cancel, default);
            StartInThread();
        }
    }
}

