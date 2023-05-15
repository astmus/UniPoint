using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissCore.Collections;

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
            Unit<TScope>.JoinData(data, Map);
            Data = data;
        } 

        IServiceProvider scoped;
        IBotServicesProvider botServices;
        public bool? IsHandled { get; set; }
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
      

        public IAsyncHandler<T> GetAsyncHandler<T>()
            => Root.BotServices.GetService<IAsyncHandler<T>>();

        public IResponse<TUnit> CreateResponse<TUnit>()
        {
            return ActivatorUtilities.GetServiceOrCreateInstance<IResponse<TUnit>>(BotServices);
        }

        //public TUnit CreateUnit<TUnit>()
        //{
        //    return ActivatorUtilities.GetServiceOrCreateInstance<Unit<TUnit>>(BotServices);
        //}

        public IHandleContext Root { get; protected set; }

        public AsyncHandler Handler
            => Get<AsyncHandler>();

        IDataMap IHandleContext.Map => Root.Map;
    }
}
