using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Abstractions.DataContext;
using MissCore.Collections;

namespace MissCore.Data.Context
{
    public class Context<TScope> : Context, IContext<TScope>, IHandleContext
    {
        IServiceProvider scoped;
        IBotServicesProvider botServices;
        public bool? IsHandled { get; set; }
        public TScope Data
        {
            get => Root.Get<TScope>();
            set => Root.Set(value);
        }
        public string Key
            => Map.Key;
        public Context(IServiceProvider scopedProvider, IBotServicesProvider botServices)
        {
            this.botServices = botServices;
            scoped = scopedProvider;
            Root = this;
        }

        public void SetData(TScope data)
        {
            if (Map is DataMap map)
                map.JoinData(data);
            else
                Map = DataMap.Parse(data);
            
            Data = data;
        }

        public IBotServicesProvider BotServices
            => botServices ?? Root.BotServices;

        public IHandleContext SetNextHandler<T>(IContext context, T data) where T : class
        {
            context.Set(data);
            return this;
        }

        public T GetNextHandler<T>() where T : class
            => Root.BotServices.GetService<T>();      

        public IAsyncHandler<T> GetAsyncHandler<T>()
            => Root.BotServices.GetService<IAsyncHandler<T>>();

        public bool Contains<T>()
            => ContainsKey(Unit<T>.Key) || Map.AllKeys?.Contains(Unit<T>.Key) == true;

        public IHandleContext Root { get; protected set; }

        public AsyncHandler CurrentHandler
            => Get<AsyncHandler>();

        public IRequestProvider Provider
            => BotServices.GetRequiredService<IRequestProvider>();

        public IBotContext Bot
            => BotServices.GetRequiredService<IBotContext>();
    }
}
