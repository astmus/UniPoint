using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Entities;

namespace MissCore.Handlers
{
    public class Handler<TBot> : IAsyncHandler<Update<TBot>> where TBot:class, IBot
    {
        class HandleContext : IHandleContext
        {
            public IBotServicesProvider BotServices { get; set; }
            public IContext ContextData { get; set; }
            public IUpdateInfo Update { get; set; }
            public T NextHandler<T>() where T : IAsyncHandler
                => BotServices.GetRequiredService<T>();
        }
        HandleDelegate handleDelegate;
        IBotServicesProvider botServices;
        public Handler(IBotBuilder<TBot> builder)
        {            
            handleDelegate = builder.BuildHandler();
            botServices = builder.BotServicesProvider();
        }

        public async Task HandleAsync(IContext<Update<TBot>> context, Update<TBot> data)
            => await handleDelegate(new HandleContext() { BotServices = botServices, ContextData = context, Update = data }).ConfigureAwait(false);        
    }    
}
