using BotService.Common;
using MissBot.Attributes;
using MissCore.Abstractions;
using MissCore.Configuration;
using MissCore.Entities;
using MissCore.Handlers;
using MissDataMaiden.Commands;
using Telegram.Bot.Types;

namespace MissDataMaiden
{

    [HasBotCommand(Name = nameof(List), Description = "List of data bases with info")]
    [HasBotCommand(Name = nameof(Info), Description = "Inforamtion about current server state")]
    [HasBotCommand(Name = nameof(Disk), Description = "Disk space information")]
    public class MissDataMaid :  IBot
    {
        private readonly ILogger<MissDataMaid> _logger;
        private IServiceScope scope;
        private ILogger<MissDataMaid> log;

        public class Handler : IAsyncHandler<Update<MissDataMaid>>
        {
            HandleDelegate Handle;
            IServiceProvider BotServices;
            public Handler(IContext<Update<MissDataMaid>> context, IBotBuilder<MissDataMaid> builder)
            {
                Context = context;
                Handle = builder.Build();
                BotServices = builder.BotServices();
            }
            IContext<Update<MissDataMaid>> Context { get; }

            public async Task HandleAsync(IContext<Update<MissDataMaid>> context, Update<MissDataMaid> data)
            {
                await Handle(new Context() { BotServices = this.BotServices, ContextData = Context, Update = data });
            }
        }
        class Context : IHandleContext
        {
            public IServiceProvider BotServices { get; set; }
            public IContext ContextData { get; set; }
            public IUpdateInfo Update { get; set; }
            public T NextHandler<T>() where T : IAsyncHandler
                => BotServices.GetRequiredService<T>();
        }
        
        public IServiceProvider BotServices
            => scope.ServiceProvider;
        public User BotInfo { get; set; }

        public Func<Update, string> ScopePredicate
            => (u) => $"{nameof(u.Message.Chat)}: {u.Message.Chat.Id}";

        public MissDataMaid(ILogger<MissDataMaid> logger, IHostApplicationLifetime lifeTime)
        {
            log = logger;
        }

        public void ConfigureOptions(IBotOptionsBuilder botBuilder)
            => botBuilder 
                        .ReceiveCallBacks()
                        .ReceiveInlineQueries()
                        .ReceiveInlineResult();                        

        private Task HandleError(Exception error, CancellationToken cancel)
        {
            log.LogError(error, error.Message);
            return Task.CompletedTask;
        }   

        public void ConfigureConnection(IBotConnectionOptionsBuilder connectionOptions)
            => connectionOptions            
                    .SetToken(Environment.GetEnvironmentVariable("JarviseKey", EnvironmentVariableTarget.User))
                    .SetTimeout(TimeSpan.FromMinutes(2))                        
                    .SetExceptionHandler(HandleError);
    }
}
