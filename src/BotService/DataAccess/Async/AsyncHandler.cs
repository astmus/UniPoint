using MissCore.Abstractions;
using MissCore.Handlers;

namespace BotService.DataAccess.Async
{
    public abstract class AsyncBotHandler<TUpdate> : IAsyncHandler where TUpdate : class, IHandleContext
    {
        protected HandleDelegate handler;
        public uint Id { get; set; }        
        public bool IsHandled { get; set; }

        protected IContext<TUpdate> context;
        protected TUpdate data;
        public void SetHandle(HandleDelegate handle)
            => handler = handle;
        public abstract Task HandleAsync(TUpdate data, IHandleContext context, CancellationToken cancel);
        public abstract Task ExecuteAsync(IHandleContext context, HandleDelegate next);
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
            return ExecuteAsync(data,handler);
        }
    }
}
