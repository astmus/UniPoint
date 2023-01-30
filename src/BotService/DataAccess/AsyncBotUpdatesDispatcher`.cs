using System.Threading;
using BotService.Common;
using BotService.DataAccess.Async;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.DataAccess;
using MissCore.DataAccess.Async;


namespace BotService.DataAccess
{


    public class AsyncBotUpdatesDispatcher<TUpdate> : BaseDataSource<TUpdate>,  IBotUpdatesDispatcher<TUpdate> where TUpdate : class, IUpdateInfo
    {
        public ILogger<AsyncBotUpdatesDispatcher<TUpdate>> log { get; protected set; }
        Thread thread;   
        protected override AsyncUpdatesQueue<TUpdate> Updates { get; init; }
        public AsyncBotUpdatesDispatcher(ILogger<AsyncBotUpdatesDispatcher<TUpdate>> logger)
        {
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
                    log.WriteJson(update);

                    //contextFactory.HandleInput(update);
                    
                    //var bot = chatScope.GetScopedBot<IBot<TUpdate>>();
                    //contextFactory.GetChatContext(update).With();

                   // var updateSctx = contextFactory.GetOrInit<IContext<TUpdate>>();
                    
                  //  var handler = bot.BotServices.GetRequiredService<IAsyncHandler<TUpdate>>();
                   // await handler.HandleAsync(updateSctx, update).ConfigFalse();
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

