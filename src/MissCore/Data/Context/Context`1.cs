using MissBot.Abstractions;
using MissCore.Configuration;

namespace MissCore.Data.Context
{
    public class Context<TScope> : Context, IContext<TScope>, IHandleContext
    {
        public TScope Data
        {
            get => Root.Get<TScope>();
            set => Root.Set(value);
        }

        public void SetServices(IBotServicesProvider botServices)
        {
            this.botServices = botServices;
        }

        IBotServicesProvider botServices;
        public IBotServicesProvider BotServices
            => botServices ?? Root.BotServices;

        IServiceProvider scoped;

        public Context(IServiceProvider scopedProvider)
        {
            scoped = scopedProvider;
            Root = this;            
        }


        public IHandleContext SetupData<T>(IContext context, T data)
        {
            context.Set(data);
            return this;
        }

        public T NextHandler<T>() where T : class
            => Root.BotServices.GetService<T>();

        public IContext<T> GetContext<T>(T data = default)
        {
            var ctx = Root.BotServices.GetService<IContext<T>>();
            ctx.Root = Root;
            ctx.Set(Any<ICommonUpdate>());
            ctx.Data = data;
            return ctx;
        }

        public IAsyncHandler<T> GetAsyncHandler<T>()
            => Root.BotServices.GetRequiredService<IAsyncHandler<T>>();

        public IResponse Response
        {
            get
            {
                var res = Root.BotServices.GetRequiredService<IResponse>();
                res.SetContext(this);
                return res;
            }
        }

        public IHandleContext Root { get; set; }
        public ICommonUpdate CommonUpdate
            => Get<ICommonUpdate>();
        public BotClientDelegate ClientDelegate
            => Root.BotServices.GetRequiredService<IBotClient>;
    }
}
