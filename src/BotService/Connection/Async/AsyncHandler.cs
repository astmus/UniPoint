using MissBot.Abstractions;
using MissCore.Abstractions;

namespace BotService.Connection.Async
{
    public abstract class AsyncBotHandler<TUpdate> : IAsyncHandler where TUpdate : class, IHandleContext
    {
        protected AsyncHandler handler;
        public uint Id { get; set; }
        public bool IsHandled { get; set; }
        public ExecuteHandler ExecuteHandler { get; }
        public AsyncHandler AsyncHandler { get; }

        protected IContext<TUpdate> context;
        protected TUpdate data;
        public void SetHandle(AsyncHandler handle)
            => handler = handle;
        public abstract Task HandleAsync(TUpdate data, IHandleContext context, CancellationToken cancel);
        public abstract Task ExecuteAsync(IHandleContext context, AsyncHandler next);
    }
    public abstract class UpdateHandler<TUpdate> : AsyncBotHandler<TUpdate> where TUpdate : class, IHandleContext
    {
        protected IBot Bot;
        public UpdateHandler(IBot bot)
        {
            Bot = bot;
        }
        public Task HandleAsync(IContext<TUpdate> context, TUpdate data)
        {
            this.context = context;
            this.data = data;
            return ExecuteAsync(data, handler);
        }
    }
}
