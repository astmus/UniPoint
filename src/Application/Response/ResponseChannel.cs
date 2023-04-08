using MediatR;
using MissBot.Abstractions;
using Telegram.Bot.Types;
using MissBot.Commands;
using MissCore.Abstractions;
using Telegram.Bot.Types.Enums;


namespace MissBot.Response
{
    public class ResponseChannel : IResponseChannel
    {
        IHandleContext ctx;
        public void SetContext(IHandleContext context)
        {
            ctx = context;
        }

        public async Task WriteAsync<T>(T data, CancellationToken cancel) where T : class
        {
            var message = ctx.Get<Message>();
            var responseMessage = await ctx.BotServices.GetRequiredService<IBotClient>().SendQueryRequestAsync(new SendResponse<T>(message.Chat.Id,data.ToString()));
            ctx.Set(responseMessage);
        }        

        public async Task SendHandlingStart()
        {
            await ctx.BotServices.GetRequiredService<IBotClient>().SendCommandAsync(ctx.Any<Telegram.Bot.Types.Chat>().SetAction(ChatAction.Typing));
        }

        public async Task UpdateAsync<T>(T data, CancellationToken cancel) where T : class
        {
            if (ctx.Get<Message<T>>() is Message<T> response)
            {
                response.Text = data.ToString();
                var updatedMessage = await ctx.BotServices.GetRequiredService<IBotClient>().SendQueryRequestAsync(response.Update());
                ctx.Set(updatedMessage);
            }
        }
    }
}
