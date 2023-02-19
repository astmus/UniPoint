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

            var message = ctx.ContextData.Get<Message>();
            var responseMessage = await ctx.BotServices.GetRequiredService<IBotClient>().SendQueryRequestAsync(message.Response<T>(data.ToString()));
            ctx.ContextData.Set(responseMessage);

        }
        public async Task<T> WriteMessageAsync<T>(T data, CancellationToken cancel) where T : class
        {

            var chat = ctx.ContextData.Get<Chat>();
            var responseMessage = await ctx.BotServices.GetRequiredService<IBotClient>().SendQueryRequestAsync(chat.Response<T>(data));
           
            ctx.ContextData.Set(responseMessage);
            return responseMessage.Data;
        }

        public async Task SendHandlingStart()
        {
            await ctx.BotServices.GetRequiredService<IBotClient>().SendCommandAsync(ctx.ContextData.Get<Chat>().SetAction(ChatAction.Typing));
        }

        public async Task UpdateAsync<T>(T data, CancellationToken cancel) where T : class
        {
            if (ctx.ContextData.Get<Message<T>>() is Message<T> response)
            {
                response.Text = data.ToString();
                var updatedMessage = await ctx.BotServices.GetRequiredService<IBotClient>().SendQueryRequestAsync(response.Update());
                ctx.ContextData.Set(updatedMessage);
            }
        }
    }
}
