using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Entities;
using MissBot.Entities.Query;
using MissBot.Entities.Results;
using MissCore.Data;

namespace MissCore.Handlers
{
    public class BotUpdateHandler<TBot> : BaseHandleComponent, IAsyncHandler<Update<TBot>> where TBot : class, IBot
    {
        private readonly IBotBuilder<TBot> builder;
        AsyncHandler handleDelegate;

        public BotUpdateHandler(IBotBuilder<TBot> builder)
        {
            this.builder = builder;
        }

        public override Task ExecuteAsync(IHandleContext context)
        {
            throw new NotImplementedException();
        }

        public async Task HandleAsync(Update<TBot> data, IHandleContext context, CancellationToken cancel = default)
        {
            handleDelegate = context.Handler;

            if (handleDelegate == null)
            {
                //context.SetServices(builder.BotServicesProvider());
                handleDelegate = builder.BuildHandler();
                context.Set(handleDelegate);
            }

            SetUpdateObject(context, data.Type);

            await handleDelegate(context).ConfigureAwait(false);
        }


        object SetUpdateObject(IHandleContext ctx, UpdateType type) => type switch
        {
            UpdateType.InlineQuery => ctx.Set(ctx.Get<InlineQuery>()),
            UpdateType.CallbackQuery => ctx.Set(ctx.Get<CallbackQuery>()),
            UpdateType.ChosenInlineResult => ctx.Set(ctx.Get<ChosenInlineResult>()),
            _ => ctx
        };

    }
}
