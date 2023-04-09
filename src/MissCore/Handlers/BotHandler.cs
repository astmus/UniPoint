using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissCore.Configuration;
using MissCore.Entities;

namespace MissCore.Handlers
{
    public class BotUpdateHandler<TBot> : BaseHandleComponent, IAsyncHandler<Update<TBot>> where TBot:class, IBot
    {
        private readonly IBotBuilder<TBot> builder;
        AsyncHandler handleDelegate;
      
        public BotUpdateHandler(IBotBuilder<TBot> builder)
        {            
            //handleDelegate = builder.BuildHandler();
            //botServices = builder.BotServicesProvider();
            this.builder = builder;
        }

        public AsyncGenericHandler<Update<TBot>> GenericHandler
            => HandleAsync;

        public override Task ExecuteAsync(IHandleContext context)
        {
            throw new NotImplementedException();
        }

        public async Task HandleAsync(IContext<Update<TBot>> context)
        {
            context.BotServices ??= builder.BotServicesProvider();
            handleDelegate ??= builder.BuildHandler();
            await handleDelegate(context).ConfigureAwait(false);
        }
    }    
}
