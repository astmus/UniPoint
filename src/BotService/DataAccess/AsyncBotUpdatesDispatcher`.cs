using System.Threading;
using BotService.Common;
using BotService.DataAccess.Async;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.DataAccess.Async;


namespace BotService.DataAccess
{


    public class AsyncBotUpdatesDispatcher<TUpdate> : BaseDataSource<TUpdate>,  IBotUpdatesDispatcher<TUpdate> where TUpdate : class, IUpdateInfo
    {
        IHandleContextFactory Factory { get; }
        public ILogger<AsyncBotUpdatesDispatcher<TUpdate>> log { get; protected set; }
        Thread thread;   
        protected override AsyncUpdatesQueue<TUpdate> Updates { get; init; }
        public Func<TUpdate, string> ScopePredicate { get; set; }

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
                    log.WriteJson(update.GetType().FullName);
                    var scope = Factory.Init(ScopePredicate(update));
                    var handler = scope.ServiceProvider.GetRequiredService<IAsyncHandler<TUpdate>>();
                    var ctx = scope.ServiceProvider.GetRequiredService<IContext<TUpdate>>();
                    ctx.Set(update);
                    await handler.HandleAsync(ctx, update);                    
                };
            }
            catch (Exception e)
            {
                log.WriteCritical(e);
            }
        }   

        CancellationTokenSource src;
        public void PushUpdate(TUpdate update)
            => Updates.QueueItem(update);

        public void Initialize(CancellationToken cancel = default)
        {
            src = CancellationTokenSource.CreateLinkedTokenSource(cancel, default);
            StartInThread();            
        }
    }   
}

