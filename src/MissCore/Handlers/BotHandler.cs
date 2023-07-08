using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Entities;
using MissBot.Entities.Query;
using MissBot.Entities.Results;
using MissCore.Data;

namespace MissCore.Handlers
{
    public class BotUpdateHandler<TBot> : IAsyncUpdateHandler<Update<TBot>> where TBot : class, IBot
    {
        private readonly IBotBuilder<TBot> builder;

        public BotUpdateHandler(IBotBuilder<TBot> builder)
        {
            this.builder = builder;
        }
        AsyncHandler startHandler;
        public async Task HandleUpdateAsync(Update<TBot> data, IHandleContext context, CancellationToken cancel)
        {
            if (context.CurrentHandler != null)
                context.IsHandled = null;
            else
                startHandler = builder.BuildHandler();

            await startHandler(context).ConfigureAwait(false);
        }

        //object SetUpdateObject(IHandleContext ctx, UpdateType type) => type switch
        //{
        //    UpdateType.Message => ctx.Set(ctx.Get<Message>()),
        //    UpdateType.InlineQuery => ctx.Set(ctx.Get<InlineQuery>()),
        //    UpdateType.CallbackQuery => ctx.Set(ctx.Get<CallbackQuery>()),
        //    UpdateType.ChosenInlineResult => ctx.Set(ctx.Get<ChosenInlineResult>()),
        //    _ => ctx
        //};

    }
}
