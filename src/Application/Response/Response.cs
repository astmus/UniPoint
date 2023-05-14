using MissBot.Abstractions;
using MissBot.Abstractions.Configuration;
using MissBot.Entities;
using MissCore.DataAccess;
using Telegram.Bot.Types.Enums;

namespace MissBot.Response
{
    public class Response : ResponseDataContext, IResponse
    {
        IHandleContext ctx;


        public async Task WriteAsync<T>(T data, CancellationToken cancel) where T : class
        {
            var message = ctx.Any<ICommonUpdate>();
            var response = ctx.BotServices.Response<T>();


            await response.Commit(cancel);
        }

        public Task SendHandlingStart()
        => Task.CompletedTask;
           // await ctx.BotServices.GetRequiredService<IBotClient>().SendCommandAsync(ctx.Any<Chat>());


        public IResponse<T> Init<T>(T data, IHandleContext context)
        {
            ctx = context;
            var message = ctx.Any<ICommonUpdate>();
            var response = ctx.BotServices.Response<T>();

            return response;
        }
    }
}
