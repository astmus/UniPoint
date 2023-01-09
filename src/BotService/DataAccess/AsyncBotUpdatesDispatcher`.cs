using System.Threading;
using BotService.Common;
using BotService.DataAccess.Async;
using MissCore.Abstractions;
using MissCore.DataAccess;
using MissCore.DataAccess.Async;


namespace BotService.DataAccess
{


    public class AsyncBotUpdatesDispatcher<TUpdate> : BaseDataSource<TUpdate>,  IBotUpdatesDispatcher<TUpdate> where TUpdate : class, IUpdateInfo
    {
        public ILogger<AsyncBotUpdatesDispatcher<TUpdate>> log { get; protected set; }
        Thread thread;

        protected DataContextFactory contextFactory { get; }
        protected override AsyncUpdatesQueue<TUpdate> Updates { get; init; }
        public AsyncBotUpdatesDispatcher(IServiceScopeFactory scopeFactory, IServiceProvider sp, ILogger<AsyncBotUpdatesDispatcher<TUpdate>> logger)
        {
             log = logger;
            //LazyQueue = new Lazy<AsyncUpdatesQueue<TUpdate>>(() =>
            //{
            //    //StartInThread();
            //    return new AsyncUpdatesQueue<TUpdate>();
            //});
            Updates = new AsyncUpdatesQueue<TUpdate>();
            contextFactory = new DataContextFactory(scopeFactory);

            thread = new Thread(StartInThread);
            thread.IsBackground = true;
        }
        
        protected async void StartInThread()
        {
            try
            {
                await foreach (var update in PendingUpdates(src.Token))
                {
                    log.WriteJson(update);

                    //contextFactory.HandleInput(update);

                    var bot = contextFactory.GetOrInit<IBot>();
                    //var bot = chatScope.GetScopedBot<IBot<TUpdate>>();
                    //contextFactory.GetChatContext(update).With();

                    var updateSctx = contextFactory.GetOrInit<IContext<TUpdate>>();
                    
                    var handler = bot.BotServices.GetRequiredService<IAsyncHandler<TUpdate>>();
                    await handler.HandleAsync(updateSctx, update).ConfigFalse();
                };
            }
            catch (Exception e)
            {
                log.WriteCritical(e);
            }
        }   

        CancellationTokenSource src;
        public override Task StopAsync(CancellationToken cancellationToken)
            => base.StopAsync(cancellationToken);

        //Lazy<AsyncUpdatesQueue<TUpdate>> LazyQueue;
        public void PushUpdate(TUpdate update)
            => Updates.QueueItem(update);

        public void Initialize(CancellationToken cancel = default)
        {
            src = CancellationTokenSource.CreateLinkedTokenSource(cancel, default);
            StartInThread();            
        }
    }   
}

