using Microsoft.Extensions.DependencyInjection;
using MissBot.Abstractions;
using MissCore.Configuration;
using MissCore.Entities;
using Telegram.Bot.Types.Enums;

namespace MissCore.Handlers
{
    public class BotUpdateHandler<TBot> : BaseHandleComponent, IAsyncHandler<Update<TBot>> where TBot:class, IBot
    {
        private readonly IBotBuilder<TBot> builder;
        AsyncHandler handleDelegate;
      
        public BotUpdateHandler(IBotBuilder<TBot> builder)
        {               
            this.builder = builder;
        }

        public AsyncGenericHandler<Update<TBot>> GenericHandler
            => HandleAsync;

        public override Task ExecuteAsync(IHandleContext context)
            => Task.CompletedTask;

        public async Task HandleAsync(IContext<Update<TBot>> context)
        {
            handleDelegate = context.Get<AsyncHandler>();

            if (handleDelegate == null)
            {
                context.SetServices(builder.BotServicesProvider());
                handleDelegate = builder.BuildHandler();
                context.Set(handleDelegate);
            }

            SetUpdateObject(context, context.Data.Type);
            
            await handleDelegate(context.Root).ConfigureAwait(false);            
        }

        object SetUpdateObject(IContext<Update<TBot>> ctx, UpdateType type) => type switch
        {
            UpdateType.InlineQuery => ctx.Set(ctx.Data.InlineQuery),
            UpdateType.CallbackQuery => ctx.Set(ctx.Data.CallbackQuery),
            _ => ctx
        };

    }    
}
