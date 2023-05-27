using MissBot.Abstractions;
using MissBot.Abstractions.DataAccess;
using MissCore.Collections;
using Telegram.Bot.Requests;

namespace MissCore.Data.Context
{
    public class Context<TScope> : Context, IContext<TScope>, IHandleContext
    {
        protected IHandleContext Root { get; set; }
        IBotServicesProvider botServices;
        public bool? IsHandled { get; set; }
        AsyncHandler currentHandler;
        public Context(IBotServicesProvider botServiceProvider)
        {
            botServices = botServiceProvider;
            Root = this;
        }
        public IBotServicesProvider BotServices
            => botServices ?? Root.BotServices;

        public IContext<TScope> SetData(TScope data)
        {
            if (Map is DataMap map)
                map.JoinData(data);
            else
                Map = DataMap.Parse(data);
            Root.Set(data);
            return this;
        }
        public IHandleContext SetNextHandler<T>(T data) where T : class
        {
            Set(data);
            return this;
        }

        public T GetBotService<T>() where T : class
            => BotServices.GetService<T>();
        public THandler GetAsyncHandler<THandler>() where THandler : class, IAsyncHandler
            => BotServices.GetService<THandler>();

        public IAsyncHandler<T> GetAsyncHandlerOf<T>()
            => BotServices.GetService<IAsyncHandler<T>>();

        public bool Contains<T>(Id<T> identifier)
            => ContainsKey(identifier.id) || Map.AllKeys?.Contains(identifier.id) == true;

        public async Task MoveToNextHandler()
        {
            currentHandler = Get<AsyncHandler>();
            await currentHandler(this).ConfigureAwait(false);
        }

        public AsyncHandler CurrentHandler
            => currentHandler;

        public IRequestProvider Provider
            => BotServices.GetRequiredService<IRequestProvider>();

        public IBotContext Bot
            => BotServices.GetRequiredService<IBotContext>();
    }
}
