using MediatR;
using MissBot.Abstractions;
using Telegram.Bot.Types;
using MissBot.Commands;
using Telegram.Bot.Types.Enums;
using MissCore.Configuration;

namespace MissBot.Response
{
    public class Response : IResponse
    {
        IHandleContext ctx;
        public void SetContext(IHandleContext context)
        {
            ctx = context;
        }

        public async Task WriteAsync<T>(T data, CancellationToken cancel) where T : class
        {
            var message = ctx.Any<ICommonUpdate>();
            var response = ctx.BotServices.Response<T>();
            response.Init(message, ctx.BotServices.GetRequiredService<IBotClient>);
            
            await response.Commit(cancel);            
        }        

        public async Task SendHandlingStart()
        {
            await ctx.BotServices.GetRequiredService<IBotClient>().SendCommandAsync(ctx.Any<Chat>().SetAction(ChatAction.Typing));
        }

        
        public async Task<IResponseChannel> CreateAsync<T>(T data, CancellationToken cancel) where T : class
        {
            var message = ctx.Any<ICommonUpdate>();
            var response = ctx.BotServices.Response<T>();
            return await response.InitAsync(data, message, ctx.BotServices.GetRequiredService<IBotClient>);

            
        }

        public IResponse<T> Create<T>(T data) where T : class
        {
            var message = ctx.Any<ICommonUpdate>();
            var response = ctx.BotServices.Response<T>();
            response.Init(message, ctx.BotServices.GetRequiredService<IBotClient>);
            return response;
        }
    }
}
