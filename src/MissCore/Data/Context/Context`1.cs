using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Entities.Common;

namespace MissCore.Data.Context
{
    public class Context<TScope> : Context, IContext<TScope>, IHandleContext
    {
        public TScope Data
        {
            get => Root.Get<TScope>();
            set => Root.Set(value);
        }

        public void SetData(TScope data)
        {
            Map = Unit<TScope>.MapData(data);
            Data = data;
        }



        IServiceProvider scoped;
        IBotServicesProvider botServices;
        public IBotServicesProvider BotServices
            => botServices ?? Root.BotServices;

        public Context(IServiceProvider scopedProvider, IBotServicesProvider botServices)
        {
            this.botServices = botServices;
            scoped = scopedProvider;
            Root = this;
        }

        public IHandleContext SetNextHandler<T>(IContext context, T data) where T : class
        {
            context.Set(data);
            return this;
        }

        public T GetNextHandler<T>() where T : class
            => Root.BotServices.GetService<T>();

        public IContext<T> CreateDataContext<T>(T data = default)
        {
            var ctx = Root.BotServices.GetService<IContext<T>>() as Context<T>;
            ctx.Root = Root;
            ctx.Set(Any<ICommonUpdate>());
            ctx.Data = data;
            return ctx;
        }

        public IAsyncHandler<T> GetAsyncHandler<T>()
            => Root.BotServices.GetService<IAsyncHandler<T>>();

        public IResponse<TSub> CreateResponse<TSub>(TScope scopeData = default) where TSub : TScope
        {
            return ActivatorUtilities.GetServiceOrCreateInstance<IResponse<TSub>>(BotServices);
        }

        public IHandleContext Root { get; protected set; }

        public BotClientDelegate ClientDelegate
            => Root.BotServices.GetRequiredService<IBotClient>;

        public AsyncHandler Handler
            => Get<AsyncHandler>();

    }
}
