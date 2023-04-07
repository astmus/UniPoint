using MissBot.Abstractions;
using MissBot.Attributes;
using MissBot.Handlers;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Entities;
using MissCore.Handlers;
using MissDataMaiden.Commands;
using Telegram.Bot.Types;

namespace MissDataMaiden
{

    [HasBotCommand(Name = nameof(List), CmdType = typeof(List), Description = "List of data bases with info")]
    [HasBotCommand(Name = nameof(Info), CmdType = typeof(Info), Description = "Inforamtion about current server state")]
    [HasBotCommand(Name = nameof(Disk), CmdType = typeof(Disk), Description = "Disk space information")]
    public class MissDataMaid : IBot
    {
        private readonly ILogger<MissDataMaid> _logger;
        private IServiceScope scope;
        private ILogger<MissDataMaid> log;

        public class UpdateHandler :  IAsyncUpdateHandler<Update<MissDataMaid>>
        {
            private readonly IBotBuilder<MissDataMaid> builder;
            AsyncHandler handleDelegate;
            public UpdateHandler(IBotBuilder<MissDataMaid> builder)
            {
                this.builder = builder;
            }

            public Update<MissDataMaid> Update { get; set; }

            public  Task HandleUpdateAsync<U>(U update, IContext<Update<MissDataMaid>> context) where U : Update<MissDataMaid>, IUpdateInfo
            {
                context.BotServices ??= builder.BotServicesProvider();
                context.Scope = update;
                handleDelegate ??= builder.BuildHandler();
                
                return Task.FromResult(ThreadPool.QueueUserWorkItem<IContext<Update<MissDataMaid>>>(async ctx
                    => await handleDelegate(ctx), context, false));
                //await handleDelegate(context);
            }          
        }

        public User BotInfo { get; set; }
        public Func<Update, string> ScopePredicate
             => (u) => u is Update<MissDataMaid> upd ? $"{nameof(upd.Chat)}: {upd.Chat.Id}" : "";

        public MissDataMaid(ILogger<MissDataMaid> logger, IHostApplicationLifetime lifeTime)
        {
            log = logger;
        }

        public void ConfigureOptions(IBotOptionsBuilder botBuilder)
            => botBuilder
                        .ReceiveCallBacks()
                        .ReceiveInlineQueries()
                        .ReceiveInlineResult()
                        .TrackMessgeChanges();

        private Task HandleError(Exception error, CancellationToken cancel)
        {
            log.LogError(error, error.Message);
            return Task.CompletedTask;
        }

        public void ConfigureConnection(IBotConnectionOptionsBuilder connectionOptions)
            => connectionOptions
                    .SetToken(Environment.GetEnvironmentVariable("JarviseKey", EnvironmentVariableTarget.User))
                    .SetTimeout(TimeSpan.FromMinutes(2));
         }
}
