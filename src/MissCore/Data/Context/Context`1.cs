using MissBot.Abstractions;
using MissBot.Abstractions.DataContext;
using MissCore.Collections;

namespace MissCore.Data.Context
{
    public class Context<TScope> : Context, IContext<TScope>, IHandleContext
    {        
        protected IHandleContext Root { get; set; }
        IBotServicesProvider botServices;
        public bool? IsHandled { get; set; }

        public Context(IBotServicesProvider botServiceProvider)
        {
            botServices = botServiceProvider;     
            Root = this;
        }

        public void SetData(TScope data)
        {
            if (Map is DataMap map)
                map.JoinData(data);
            else
                Map = DataMap.Parse(data);
            Root.Set(data);
        }

        public IBotServicesProvider BotServices
            => botServices ?? Root.BotServices;

        public IHandleContext SetNextHandler<T>(IContext context, T data) where T : class
        {
            context.Set(data);
            return this;
        }

        public T GetBotService<T>() where T : class
            => Root.BotServices.GetService<T>();      

        public IAsyncHandler<T> GetAsyncHandler<T>()
            => Root.BotServices.GetService<IAsyncHandler<T>>();

        public bool Contains<T>()
            => ContainsKey(Unit<T>.Key) || Map.AllKeys?.Contains(Unit<T>.Key) == true;

        public AsyncHandler CurrentHandler
            => Get<AsyncHandler>();

        public IRequestProvider Provider
            => BotServices.GetRequiredService<IRequestProvider>();

        public IBotContext Bot
            => BotServices.GetRequiredService<IBotContext>();
    }
}
