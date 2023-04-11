using MediatR;
using MissBot.Abstractions;
using Telegram.Bot.Types;
using MissBot.Commands;
using Telegram.Bot.Types.Enums;
using MissBot.Abstractions.Configuration;
using MissCore.DataAccess;

namespace MissBot.Response
{
    public class Response : ResponseDataContext, IResponse
    {
        IHandleContext ctx;
       

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

        
        public async Task<IResponseChannel> InitAsync<T>(T data, CancellationToken cancel, IHandleContext context) where T : class
        {
            ctx = context;
            var message = context.Any<ICommonUpdate>();
            var response = ctx.BotServices.Response<T>();
            return await response.InitAsync(data, message, ctx.BotServices.GetRequiredService<IBotClient>);            
        }

        public IResponse<T> Init<T>(T data, IHandleContext context)
        {
            ctx = context;
            var message = ctx.Any<ICommonUpdate>();
            var response = ctx.BotServices.Response<T>();
            response.Init(message, ctx.BotServices.GetRequiredService<IBotClient>, data);
            return response;
        }
    }
}
