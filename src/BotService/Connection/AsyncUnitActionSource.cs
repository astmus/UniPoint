using MissBot.Abstractions;
using MissBot.Abstractions.Bot;
using MissBot.Entities;
using MissBot.Extensions;
using MissCore.Handlers;

namespace BotService.Connection
{
    public class AsyncUnitActionSource<TUnit> : BaseDataSource<TUnit>, IAsyncUnitActionSource<TUnit> where TUnit : BaseBotUnit
    {
        IHandleContextFactory context { get; }
        public ILogger<AsyncUnitActionSource<TUnit>> log { get; protected set; }
        Thread thread;
        protected override AsyncItemsQueue<TUnit> Items { get; init; }
        CancellationTokenSource src;

        public AsyncUnitActionSource(IHandleContextFactory handleContext, ILogger<AsyncUnitActionSource<TUnit>> logger)
        {
            context = handleContext;
            log = logger;
            Items = new AsyncItemsQueue<TUnit>();
            thread = new Thread(StartInThread);
            thread.IsBackground = true;
            init = Initialize;
        }

        protected async void StartInThread()
        {
            try
            {
                await foreach (var item in PendingItems(src.Token))
                {
                    //if (ScopePredicate(update) is not string id) continue;

                    using (var cts = new CancellationTokenSource())
                    {
                        //var handler = context.GetBotService<BotUnitCommandHandler>();

                        //await handler.AsyncDelegate(context).ConfigFalse();                        
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

        Action<CancellationToken> init;
        public void PushUnit(TUnit unit, IHandleContext context, CancellationToken cancel = default)
        {
            init?.Invoke(cancel);
            Items.QueueItem(unit);
        }

        public void Initialize(CancellationToken cancel = default)
        {
            src = CancellationTokenSource.CreateLinkedTokenSource(cancel, default);
            StartInThread();
            init = null;
        }
    }
}

