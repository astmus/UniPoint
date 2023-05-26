using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissCore.Collections;

namespace MissCore.Data.Context
{
    public class Context<TScope> : Context, IContext<TScope>, IHandleContext
    {        
        protected IHandleContext Root { get; set; }
        IBotServicesProvider botServices;
        public bool? IsHandled { get; set; }
        IAsyncHandler currentHandler;
        public Context(IBotServicesProvider botServiceProvider)
        {
            botServices = botServiceProvider;     
            Root = this;
        }

        public IContext<TScope> SetData(TScope data)
        {
            if (Map is DataMap map)
                map.JoinData(data);
            else
                Map = DataMap.Parse(data);
            Root.Set(data);
            return this;
        }

        public IBotServicesProvider BotServices
            => botServices ?? Root.BotServices;

        public IHandleContext SetNextHandler<T>(IContext context, T data) where T : class
        {
            context.Set(data);
            return this;
        }

        public T GetBotService<T>() where T : class
            => BotServices.GetService<T>();
        public THandler GetAsyncHandler<THandler>() where THandler: class, IAsyncHandler
        {
            var handler = BotServices.GetService<THandler>();
            currentHandler = handler;
            return handler;
        }
        public IAsyncHandler<T> GetAsyncHandlerOf<T>()
        {
            var handler = BotServices.GetService<IAsyncHandler<T>>();
            currentHandler = handler;
            return handler;
        }

        public bool Contains<T>(Id<T> identifier)
            => ContainsKey(identifier.id) || Map.AllKeys?.Contains(identifier.id) == true;

        public async Task MoveToNextHandler()
        {
            await Get<AsyncHandler>()(this).ConfigureAwait(false);
        }

        public AsyncHandler CurrentHandler
            => currentHandler?.AsyncDelegate;

        public IRequestProvider Provider
            => BotServices.GetRequiredService<IRequestProvider>();

        public IBotContext Bot
            => BotServices.GetRequiredService<IBotContext>();
    }
}
