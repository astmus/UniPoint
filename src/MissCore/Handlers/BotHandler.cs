using Microsoft.Extensions.DependencyInjection;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Entities;
using MissCore.Handlers;

namespace MissCore.Handlers
{
    public class Handler<TBot> : IAsyncHandler<Update<TBot>> where TBot:class, IBot
    {
        class HandleContext : IHandleContext
        {
            public IServiceProvider BotServices { get; set; }
            public IContext ContextData { get; set; }
            public IUpdateInfo Update { get; set; }
            public T NextHandler<T>() where T : IAsyncHandler
                => BotServices.GetRequiredService<T>();
        }
        HandleDelegate Handle;
        IServiceProvider BotServices;
        public Handler(IContext<Update<TBot>> context, IBotBuilder<TBot> builder)
        {
            Context = context;
            Handle = builder.Build();
            BotServices = builder.BotServices();
        }
        IContext<Update<TBot>> Context { get; }

        public async Task HandleAsync(IContext<Update<TBot>> context, Update<TBot> data)
        {
            await Handle(new HandleContext() { BotServices = this.BotServices, ContextData = Context, Update = data });
        }
    }
    public class BotHandler<TBot> : IBotHandler<TBot> where TBot:class,IBot
    {
        public IBotBuilder<TBot> Builder { get; }
        public IContext<TBot> Context { get; }

        public BotHandler(IBotBuilder<TBot> builder, IContext<TBot> context)
        {
            Builder = builder;
            Context = context;
        }
        public Task StartHandle(IContext<TBot> context, Update<TBot> update)
        {
            
            //await Handler(builder.BuildBot()..CreateHandleContext(update));
            return Task.CompletedTask;
        }

        public Task HandleAsync(IContext<Update<TBot>> context, Update<TBot> data)
        {
            throw new NotImplementedException();
        }        
    }
}
