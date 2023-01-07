using BotService.Common;
using BotService.DataAccess.Async;
using MissCore.Abstractions;
using MissCore.DataAccess;

namespace BotService.DataAccess
{
    public class BotUpdatesDispatcher<TUpdate> : BaseDataSource<TUpdate> where TUpdate : class, IUpdateInfo
    {
        public ILogger<BotUpdatesDispatcher<TUpdate>> log { get; protected set; }
        Thread thread;

        protected DataContextFactory contextFactory { get; }
        protected override AsyncSourceUpdatesQueue Updates { get; }

        public BotUpdatesDispatcher(IServiceScopeFactory scopeFactory, IServiceProvider sp, ILogger<BotUpdatesDispatcher<TUpdate>> logger)
        {
            log = logger;
            Updates = new AsyncSourceUpdatesQueue();
            contextFactory = new DataContextFactory(scopeFactory);

            thread = new Thread(StartInThread);
            thread.IsBackground = true;
        }
        protected void StartInThread()
        {
            try
            {
                ExecuteAsync(src.Token).Start();
            }
            catch (Exception e)
            {
                log.WriteCritical(e);
            }
        }

        protected async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            await foreach (var update in PendingUpdates(cancellationToken))
            {
                log.WriteJson(update);

                //contextFactory.HandleInput(update);

                var bot = contextFactory.GetOrInit<IBot>();
                //var bot = chatScope.GetScopedBot<IBot<TUpdate>>();
                //contextFactory.GetChatContext(update).With();

                var updateSctx = contextFactory.GetContext<TUpdate>();
                
                var handler = bot.BotServices.GetRequiredService<IAsyncHandler<TUpdate>>();
                await handler.HandleAsync(updateSctx, update).ConfigFalse();               
            };
        }

        CancellationTokenSource src;
        public override Task StopAsync(CancellationToken cancellationToken)
            => base.StopAsync(cancellationToken);

        public void PushUpdate(TUpdate update)
            => Updates.PushUpdate(update);

        private class AsyncLazy<T> : Lazy<Task<T>>
        {
            public AsyncLazy(Func<Task<T>> valueFactory) : base(valueFactory)
            {
            }
        }
    }
}

