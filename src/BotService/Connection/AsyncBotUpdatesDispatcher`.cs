using BotService.Common;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissCore.Data.Context;
using Newtonsoft.Json.Linq;
using Telegram.Bot.Types;

namespace BotService.Connection
{
    public class AsyncBotUpdatesDispatcher<TUpdate> : BaseDataSource<TUpdate>,  IBotUpdatesDispatcher<TUpdate> where TUpdate : class, IUpdateInfo
    {
        IHandleContextFactory Factory { get; }
        public ILogger<AsyncBotUpdatesDispatcher<TUpdate>> log { get; protected set; }
        Thread thread;   
        protected override AsyncUpdatesQueue<TUpdate> Updates { get; init; }
        public Func<TUpdate, string> ScopePredicate { get; set; }

        public AsyncBotUpdatesDispatcher(IHandleContextFactory factory, ILogger<AsyncBotUpdatesDispatcher<TUpdate>> logger, IBotConnectionOptionsBuilder b)
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
                    var id = ScopePredicate(update);
                    var scope = Factory.Init(id);
                    var handler = scope.ServiceProvider.GetRequiredService<IAsyncHandler<TUpdate>>();
                    var ctx = scope.ServiceProvider.GetRequiredService<IContext<TUpdate>>();
                    ctx.SetData(update);                    
                    ctx.Set(scope.ServiceProvider);

                    await handler.HandleAsync(update, ctx as IHandleContext).ConfigFalse();

                    if (update.IsHandled == true)
                        Factory.Remove(id);
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally {
                StartInThread();
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

